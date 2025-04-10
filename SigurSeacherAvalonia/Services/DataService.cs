using MySql.Data.MySqlClient;
using SigurSeacherAvalonia.Configurations;
using SigurSeacherAvalonia.Models;
using SigurSeacherAvalonia.Models.Filters;
using System;
using System.Collections.Generic;

namespace SigurSeacherAvalonia.Services;

internal sealed class DataService
{
    public bool TryGetCars ( DataFilter filter, out string error, out List<CarPass> carPasses )
    {
        string query = GenerateQuery (filter);
        return TryExecute (query, out carPasses, out error);
    }


    private static bool TryExecute ( string query, out List<CarPass> carPasses, out string error )
    {
        using MySqlConnection connection = new (Configuration.Instance.ConnectionString);

        error = string.Empty;
        carPasses = new ();

        try
        {
            connection.Open ();
        }
        catch
        {
            error = "Подключение к базе данных невозможно. \nОбратитесь в отдел технического обеспечения по тел. 324-646";
            
            return false;
        }

        try
        {
            using MySqlCommand command = new (query, connection);
            using MySqlDataReader reader = command.ExecuteReader ();

            if ( reader.HasRows )
            {
                while ( reader.Read () )
                {
                    string num = reader.IsDBNull(0) ? string.Empty : reader.GetString(0);
                    string model = reader.IsDBNull (1) ? string.Empty : reader.GetString (1);
                    DateOnly? expirationDate = reader.IsDBNull (2)
                                              ? null
                                              : DateOnly.FromDateTime ( reader.GetDateTime (2));
                    string name = reader.IsDBNull (3) ? string.Empty : reader.GetString (3);
                    string comment = reader.IsDBNull (4) ? string.Empty : reader.GetString (4);

                    carPasses.Add (new CarPass (num, model, expirationDate, name, comment));
                }
            }
        }
        catch
        {
            error = "Запрос к базе данных не может быть обработан. \nОбратитесь в отдел технического обеспечения по тел. 324-646";
            
            return false;
        }

        return true;
    }


    private static string GenerateQuery ( DataFilter filter )
    {
        return $@"
                SELECT
                	personal.`NAME` as Number,
	                personal.POS as Model,
	                personal.EXPTIME as DateEnd,
                    sideparamvalues.VALUE as Person,
                    personal.Description as Comment
                FROM
	                personal
                LEFT JOIN sideparamvalues on personal.ID = sideparamvalues.OBJ_ID
                WHERE 
	                personal.PARENT_ID in (3, 4)
                    AND personal.`STATUS` = 'AVAILABLE'
                    {( filter.HasExpired ? "" : $"AND personal.EXPTIME >= '{DateTime.Now.Date:yyyy-MM-dd}'" )}
	                AND personal.`NAME` like '%{filter.Name}%'
                    AND sideparamvalues.`param_idx` = 0
                    AND sideparamvalues.`TABLE_ID` = 0;
            ;";
    }
}