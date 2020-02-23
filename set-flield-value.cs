private static bool TrySetValue<T>(
    Expression<Func<T>> expr,
    T newValue)
{
    var memberExpression = expr.Body as MemberExpression ?? throw new ArgumentException("Invalid expression", nameof(expr));
    var parentExpression = memberExpression.Expression as MemberExpression ?? throw new ArgumentException("Invalid expression", nameof(expr));
    var instanceExpression = memberExpression.Member as PropertyInfo ?? throw new ArgumentException("Invalid expression", nameof(expr));
    var parentFieldInfo = parentExpression.Member as FieldInfo ?? throw new ArgumentException("Invalid expression", nameof(expr));
    var captureConst = parentExpression.Expression as ConstantExpression ?? throw new ArgumentException("Invalid expression", nameof(expr));

    var parent = parentFieldInfo.GetValue(captureConst.Value);
    var value = instanceExpression.GetValue(parent);

    if (!object.Equals(value, newValue))
    {
        instanceExpression.SetValue(parent, newValue);
        return true;
    }

    return false;
}