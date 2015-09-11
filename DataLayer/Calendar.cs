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
    public class CalendarEvent
    {
        public int EventId { get; }
        public int EventTypeId { get; }
        public string EventType { get; }
        public DateTime EventDateTime { get; }
        public string  EventTitle{ get; }
        public string  EventDescription{ get; }
        public string  EventVenue { get; }

        public CalendarEvent(int eventId, int eventTypeId, string eventType, DateTime eventDateTime, string eventTitle, string eventDescription, string eventVenue)
        {
            EventId = eventId;
            EventTypeId = eventTypeId;
            EventType = EventType;
            EventDateTime = eventDateTime;
            EventTitle = eventTitle;
            EventDescription = eventDescription;
            EventVenue = eventVenue;
        }
    }


        public class Calendar : IEnumerable<CalendarEvent> 
        {
            private List<CalendarEvent> mEvents;

            public Calendar()
            {
                mEvents = new List<CalendarEvent>();
                using (SqlConnection connection = DB.SqlConnection)
                {
                    connection.Open();
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "Select * from CalendarView WHERE EvendDateTime=@today Order by EventDateTime;";
                        SqlParameter today = new SqlParameter("@today", SqlDbType.DateTime2, 0);
                        today.Value = DateTime.Now.Date;
                        command.Parameters.Add(today);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                mEvents.Add(new CalendarEvent(
                                    (int)reader["EventId"],
                                    (int)reader["EventTypeId"],
                                    (string)reader["EventType"],
                                    (DateTime)reader["EventDateTime"],
                                    (string)reader["EventTitle"],
                                    (string) reader["EventDescription"],
                                    (string)reader["EventVenue"])
                                    );
                            }
                        }
                    }
                }
            }

            public static CalendarEvent GetEvent(int id)
            {
                using (SqlConnection connection = DB.SqlConnection)
                {
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "SELECT * FROM CalendarView WHERE EventId=@id;";

                        SqlParameter idParam = new SqlParameter("@id", SqlDbType.Int);
                        idParam.Value = id;
                        command.Parameters.Add(idParam);

                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new CalendarEvent(
                                    (int)reader["EventId"],
                                    (int)reader["EventTypeId"],
                                    (string)reader["EventType"],
                                    (DateTime)reader["EventDateTime"],
                                    (string)reader["EventTitle"],
                                    (string)reader["EventDescription"],
                                    (string)reader["EventVenue"]);
                            }
                        }

                        return null;
                    }
                }
            }

            public static int UpdateEvent(CalendarEvent updatedEvent)
            {
                if (updatedEvent==null) return 0;

                using (SqlConnection connection = DB.SqlConnection)
                {
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = @"UPDATE Calendar SET EventTypeId=@typeId, EventDateTime=@eventDateTime, EventTitle=@eventTitle, EventDescription=@eventDescription, EventVenue=@eventVenue WHERE EventId=@id;";
                        command.Parameters.AddWithValue("@typeId", updatedEvent.EventTypeId);
                        command.Parameters.AddWithValue("@eventDateTime", updatedEvent.EventDateTime);
                        command.Parameters.AddWithValue("@eventTitle", updatedEvent.EventTitle);
                        command.Parameters.AddWithValue("@eventDescription", updatedEvent.EventDescription);
                        command.Parameters.AddWithValue("@eventVenue", updatedEvent.EventVenue); 

                        connection.Open();
                        return command.ExecuteNonQuery();
                    }
                }
            }

            public static int AddEvent(CalendarEvent newEvent)
            {
                if (newEvent == null) return 0;

                using (SqlConnection connection = DB.SqlConnection)
                {
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = @"INSERT INTO Calendar VALUES(@typeId, @eventDateTime, @eventTitle, @eventDescription, @eventVenue);";
                        command.Parameters.AddWithValue("@typeId", newEvent.EventTypeId);
                        command.Parameters.AddWithValue("@eventDateTime", newEvent.EventDateTime);
                        command.Parameters.AddWithValue("@eventTitle", newEvent.EventTitle);
                        command.Parameters.AddWithValue("@eventDescription", newEvent.EventDescription);
                        command.Parameters.AddWithValue("@eventVenue", newEvent.EventVenue);

                        connection.Open();
                        return command.ExecuteNonQuery();
                    }
                }
            }


            public static int RemoveEvent(int eventId)
            {
                using (SqlConnection connection = DB.SqlConnection)
                {
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "DELETE Calendar WHERE eventId=@id;";

                        SqlParameter paramId = new SqlParameter("@id", SqlDbType.Int);
                        paramId.Value = eventId;
                        command.Parameters.Add(paramId);

                        connection.Open();

                        return command.ExecuteNonQuery();
                    }
                }
            }

            public IEnumerator<CalendarEvent> GetEnumerator()
            {
                return mEvents.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return mEvents.GetEnumerator();
            }
        }
    }

