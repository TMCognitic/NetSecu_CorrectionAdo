using System.Reflection;

namespace Tools
{
    public class Command
    {
        internal string Query { get; init; }
        internal bool IsStoredProcedure { get; init; }
        internal Dictionary<string, object> Parameters { get; init; }

        public Command(string query, bool isStoredProcedure)
        {
            Query = query;
            IsStoredProcedure = isStoredProcedure;
            Parameters = new Dictionary<string, object>();
        }

        //For Ryan!!!
        public Command(string query, bool isStoredProcedure, object? parameters)
            : this(query, isStoredProcedure)
        {
            if(parameters is not null)
            {
                Type parametersType = parameters.GetType();

                foreach (PropertyInfo propertyInfo in parametersType.GetProperties())
                {
                    if(propertyInfo.CanRead)
                    {
                        Parameters.Add(propertyInfo.Name, propertyInfo.GetMethod?.Invoke(parameters, null) ?? DBNull.Value);
                    }
                }
            }
        }

        public void AddParameter(string parameterName, object? value)
        {
            Parameters.Add(parameterName, value ?? DBNull.Value);
        }
    }
}