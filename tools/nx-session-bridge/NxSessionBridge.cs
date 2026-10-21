using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Web.Script.Serialization;
using NXOpen;

// Project-local transport around Siemens NXOpen, not an official Siemens MCP.
public class NxSessionBridge
{
    private static System.Windows.Forms.Timer timer;
    private static readonly JavaScriptSerializer json = new JavaScriptSerializer();
    private static readonly string home = Path.GetDirectoryName(typeof(NxSessionBridge).Assembly.Location);
    private static readonly string state = Path.Combine(home, "state");
    private static readonly string sessionId = Guid.NewGuid().ToString("N");
    private static bool busy;
    private static DateTime lastHeartbeat = DateTime.MinValue;

    public static void Start()
    {
        if (timer != null) return;
        Directory.CreateDirectory(state);
        Directory.CreateDirectory(Path.Combine(state, "queue"));
        Directory.CreateDirectory(Path.Combine(state, "results"));
        timer = new System.Windows.Forms.Timer();
        timer.Interval = 500;
        timer.Tick += Tick;
        Heartbeat("ready");
        timer.Start();
    }

    private static object PartInfo(Part part)
    {
        if (part == null) return null;
        var components = new List<object>();
        var root = part.ComponentAssembly.RootComponent;
        if (root != null)
            foreach (var component in root.GetChildren())
                components.Add(new { name = component.DisplayName });
        return new { name = part.Name, path = part.FullPath,
            units = part.PartUnits.ToString(), components = components };
    }

    private static object Inspect()
    {
        var session = Session.GetSession();
        return new { work_part = PartInfo(session.Parts.Work),
            display_part = PartInfo(session.Parts.Display) };
    }

    private static void Write(string path, object value)
    {
        string temporary = path + ".tmp";
        File.WriteAllText(temporary, json.Serialize(value));
        if (File.Exists(path)) File.Replace(temporary, path, null);
        else File.Move(temporary, path);
    }

    private static void Heartbeat(string status)
    {
        Write(Path.Combine(state, "heartbeat.json"), new {
            session_id = sessionId, process_id = Process.GetCurrentProcess().Id,
            utc = DateTime.UtcNow.ToString("o"), status = status,
            implementation = "project_local_nxopen_ui_timer", snapshot = Inspect()
        });
        lastHeartbeat = DateTime.UtcNow;
    }

    private static string Text(Dictionary<string, object> request, string key)
    {
        return Convert.ToString(request[key]);
    }

    private static string SafeScript(string relative)
    {
        string root = Path.GetFullPath(Path.Combine(home, "commands")) + Path.DirectorySeparatorChar;
        string path = Path.GetFullPath(Path.Combine(root, relative));
        if (!path.StartsWith(root, StringComparison.OrdinalIgnoreCase) ||
            !path.EndsWith(".py", StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException("Only Python scripts inside commands are allowed.");
        for (var item = new FileInfo(path) as FileSystemInfo; item != null; )
        {
            if ((item.Attributes & FileAttributes.ReparsePoint) != 0)
                throw new InvalidOperationException("Reparse points are not allowed.");
            item = item is FileInfo ? ((FileInfo)item).Directory : ((DirectoryInfo)item).Parent;
        }
        return path;
    }

    private static object Dispatch(Dictionary<string, object> request)
    {
        if (Text(request, "session_id") != sessionId)
            throw new InvalidOperationException("NX session changed; inspect before retrying.");
        var expiry = DateTime.Parse(Text(request, "expires_utc"), null,
            System.Globalization.DateTimeStyles.RoundtripKind).ToUniversalTime();
        if (DateTime.UtcNow > expiry) throw new InvalidOperationException("Request expired.");
        string operation = Text(request, "operation");
        if (operation == "inspect") return Inspect();
        if (operation == "stop")
        {
            timer.Stop();
            Heartbeat("stopped");
            return new { stopped = true };
        }
        if (operation != "run_python") throw new InvalidOperationException("Unknown operation.");
        var session = Session.GetSession();
        string actual = session.Parts.Work == null ? "" : session.Parts.Work.FullPath;
        if (!String.Equals(actual, Text(request, "expected_work_part"), StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException("Work part changed; inspect before executing.");
        string script = SafeScript(Text(request, "script"));
        string hash;
        using (var sha = SHA256.Create())
            hash = BitConverter.ToString(sha.ComputeHash(File.ReadAllBytes(script))).Replace("-", "").ToLowerInvariant();
        if (hash != Text(request, "sha256")) throw new InvalidOperationException("Script hash changed.");
        session.ExecuteWithStringArguments(script, "main", new string[0]);
        return new { executed = script, snapshot = Inspect() };
    }

    private static void Tick(object sender, EventArgs args)
    {
        if (busy) return;
        busy = true;
        try
        {
            if ((DateTime.UtcNow - lastHeartbeat).TotalSeconds >= 2) Heartbeat("ready");
            string[] pending = Directory.GetFiles(Path.Combine(state, "queue"), "*.json");
            Array.Sort(pending, StringComparer.Ordinal);
            if (pending.Length == 0) return;
            string path = pending[0];
            string id = Path.GetFileNameWithoutExtension(path);
            string claimed = path + ".processing";
            File.Move(path, claimed);
            object result;
            try
            {
                var request = json.Deserialize<Dictionary<string, object>>(File.ReadAllText(claimed));
                result = new { id = id, session_id = sessionId, status = "PASS", data = Dispatch(request) };
            }
            catch (Exception error)
            {
                result = new { id = id, session_id = sessionId, status = "FAIL", error = error.ToString() };
            }
            Write(Path.Combine(state, "results", id + ".json"), result);
        }
        catch (Exception error)
        {
            File.WriteAllText(Path.Combine(state, "bridge_error.txt"), error.ToString());
            timer.Stop();
        }
        finally { busy = false; }
    }

    public static int GetUnloadOption(string dummy)
    {
        return (int)Session.LibraryUnloadOption.AtTermination;
    }
}
