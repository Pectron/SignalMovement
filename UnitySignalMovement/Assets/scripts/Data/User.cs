public static class User
{
    public static string Id { get; private set; } = "Default";

    public static void SetId(string id) => Id = id;
}
