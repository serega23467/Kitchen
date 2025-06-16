using UnityEngine;

public static class ComponentGetter
{
    public static bool TryGetComponent<T>(Collider col, out T component)
    {
        component = default(T);
        GameObject go = col.gameObject;
        if (go.TryGetComponent(out T component1))
        {
            component = component1;
            return true;
        }
        else if (go.transform.parent.TryGetComponent(out T component2))
        {
            component = component2;
            return true;
        }
        return false;
    }
    public static bool TryGetComponent<T>(GameObject go, out T component)
    {
        component = default(T);
        if (go.TryGetComponent(out T component1))
        {
            component = component1;
            return true;
        }
        else if (go.transform.parent.TryGetComponent(out T component2))
        {
            component = component2;
            return true;
        }
        return false;
    }
}
