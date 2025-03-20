
namespace AuthenticationService.Application.Attributes
{
    /// <summary>
    /// A marker attribute to indicate that a property should be ignored when building query predicates.
    /// </summary>
    /// <remarks>
    /// This attribute is typically used in dynamic query generation to exclude certain properties from being included
    /// in filtering or predicate conditions.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Property)]
    public class IgnoreInQueryPredicateAttribute : Attribute
    {
    }
}
