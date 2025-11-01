namespace MyOrm.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public sealed class KeyAttribute : Attribute
{
    public KeyAttribute(bool identity = true)
    {
        Identity = identity;
    }

    public bool Identity { get; }
}