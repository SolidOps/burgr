using SolidOps.UM.Shared.Domain.Entities;
using System.Security.Cryptography;
using System.Text;

namespace SolidOps.UM.Shared.Presentation.ETag;

public class ETagRepository<TEntity, T> : IETagRepository<TEntity, T>
    where TEntity : BaseDomainEntity<T>
    where T : struct
{
    private readonly object lockObj = new object();
    Dictionary<string, string> etags = new Dictionary<string, string>();
    string wholeTableETag = null;
    string typeName;
    public bool IsEnabled { get; set; }
    public ETagRepository()
    {
        typeName = typeof(T).FullName;
    }

    public void ChangeETag(string id)
    {
        lock (lockObj)
        {
             etags[id] = GenerateETag();
        }
    }

    public void ChangeWholeTableETag()
    {
        wholeTableETag = GenerateETag();
    }

    public void RemoveETag(string id)
    {
        lock (lockObj)
        {
            etags.Remove(id);
        }
    }

    public string GetByIdETag(string userId, string id)
    {
        lock (lockObj)
        {
            
            if (etags.TryGetValue(id, out var etag) && etag != null)
            {
                return Hash(userId + etag);
            }
            return null;
        }
    }

    public string GetByQueryETag(string userId)
    {
        if (wholeTableETag != null)
        {
            return Hash(userId + wholeTableETag);
        }
        return null;
    }

    public void Reset()
    {
        lock (lockObj)
        {
            var ids = etags.Keys;
            foreach (var id in ids.ToArray())
            {
                ChangeETag(id);
            }
            ChangeWholeTableETag();
        }
    }

    public void Clear()
    {
        lock (lockObj)
        {
            etags.Clear();
            wholeTableETag = null;
        }
    }

    private static string GenerateETag()
    {
        return Hash(DateTime.Now.Ticks.ToString());
    }

    private static string Hash(string value)
    {
        byte[] data = Encoding.UTF8.GetBytes(value);
        using (var md5 = MD5.Create())
        {
            var hash = md5.ComputeHash(data);
            string hex = BitConverter.ToString(hash);
            return "\"" + hex.Replace("-", "") + "\"";
        }
    }
}
