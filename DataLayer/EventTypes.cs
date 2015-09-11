using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class EventType
    {
        public int TypeId { get; }
        public string EventName{ get; }
    

        public EventType(int typeId,  string eventName)
        {
            TypeId = typeId;
            EventName = eventName;

        }
    }


    public class EventTypes : IEnumerable<EventType>
    {
        private List<EventType> mEventTypes;

        public EventTypes()
        {
            mEventTypes = new List<EventType>();
            using (SqlConnection connection = DB.SqlConnection)
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "Select * from EventTypes Order by EventType;";

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            mEventTypes.Add(new EventType(
                                (int)reader["TypeId"],
                                (string)reader["EventType"])
                                );
                        }
                    }
                }
            }
        }

        public static EventType GetEventType(int id)
        {
            using (SqlConnection connection = DB.SqlConnection)
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM EventTypes WHERE Id=@id;";

                    SqlParameter idParam = new SqlParameter("@id", SqlDbType.Int);
                    idParam.Value = id;
                    command.Parameters.Add(idParam);

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new EventType(
                                (int)reader["TypeId"],
                                (string)reader["EventType"]);
                        }
                    }

                    return null;
                }
            }
        }

        public static int UpdateEventType(EventType updatedEventType)
        {
            if (updatedEventType == null) return 0;

            using (SqlConnection connection = DB.SqlConnection)
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"UPDATE EventTypes SET EventType=@eventType WHERE Id=@id;";
                    command.Parameters.AddWithValue("@eventType", updatedEventType.EventName);

                    connection.Open();
                    return command.ExecuteNonQuery();
                }
            }
        }

        public static int AddEventType(string newEventType)
        {
            if (string.IsNullOrWhiteSpace(newEventType)) return 0;

            using (SqlConnection connection = DB.SqlConnection)
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"INSERT INTO EventTypes VALUES(@eventType);";
                    command.Parameters.AddWithValue("@eventType", newEventType);

                    connection.Open();
                    return command.ExecuteNonQuery();
                }
            }
        }


        public static int RemoveEventType(int typeId)
        {
            using (SqlConnection connection = DB.SqlConnection)
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "DELETE EvenTypes WHERE Id=@id;";

                    SqlParameter paramId = new SqlParameter("@id", SqlDbType.Int);
                    paramId.Value = typeId;
                    command.Parameters.Add(paramId);

                    connection.Open();

                    return command.ExecuteNonQuery();
                }
            }
        }

        public IEnumerator<EventType> GetEnumerator()
        {
            return mEventTypes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return mEventTypes.GetEnumerator();
        }
    }
}

