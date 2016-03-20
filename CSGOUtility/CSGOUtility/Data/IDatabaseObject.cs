namespace CSGOUtility.Data
{
    public interface IDatabaseObject
    {
        string GetSqlInsertString();
        string GetSqlReadString();
    }
}
