namespace SolidOps.UM.Shared.Presentation.ETag;

public class ETagConfiguration
{
    private Dictionary<Type, List<Type>> _dependencyTypesByETagsManagedTypes = new Dictionary<Type, List<Type>>();
    private Dictionary<Type, List<Type>> _eTagsManagedTypesByDependencyTypes = new Dictionary<Type, List<Type>>();

    public void ManageEtagsFor(Type eTagsManagedType, List<Type> dependencies = null)
    {
        if (_dependencyTypesByETagsManagedTypes.ContainsKey(eTagsManagedType))
        {
            if (dependencies != null)
            {
                if (_dependencyTypesByETagsManagedTypes[eTagsManagedType] == null)
                {
                    _dependencyTypesByETagsManagedTypes[eTagsManagedType] = dependencies;
                }
                else
                {
                    foreach (var eventType in dependencies)
                    {
                        if (!_dependencyTypesByETagsManagedTypes[eTagsManagedType].Contains(eventType))
                        {
                            _dependencyTypesByETagsManagedTypes[eTagsManagedType].Add(eventType);
                        }
                    }
                }
            }
        }
        else
        {
            _dependencyTypesByETagsManagedTypes.Add(eTagsManagedType, dependencies);
        }
        if (dependencies != null)
        {
            foreach (var eventType in dependencies)
            {
                if (!_eTagsManagedTypesByDependencyTypes.ContainsKey(eventType))
                {
                    _eTagsManagedTypesByDependencyTypes.Add(eventType, new List<Type>());
                }

                if (!_eTagsManagedTypesByDependencyTypes[eventType].Contains(eTagsManagedType))
                {
                    _eTagsManagedTypesByDependencyTypes[eventType].Add(eTagsManagedType);
                }
            }
        }
    }

    public bool AreETagsManagedForType(Type type)
    {
        return _dependencyTypesByETagsManagedTypes.ContainsKey(type);
    }

    public ISet<Type> GetETagsManagedTypesDependingOf(Type type)
    {
        void LoadDependencies(Type dependencyType, ISet<Type> loadedDependencies)
        {
            if (_eTagsManagedTypesByDependencyTypes.TryGetValue(dependencyType, out var directDependencies))
                foreach (var d in directDependencies)
                    if (loadedDependencies.Add(d))
                        LoadDependencies(d, loadedDependencies);
        }

        var dependencies = new HashSet<Type>();
        LoadDependencies(type, dependencies);
        return dependencies;
    }
}
