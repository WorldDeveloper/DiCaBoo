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
        public DateTime EventStart { get; }
        public DateTime EventEnd { get; }
        public string EventTitle { get; }
        public string EventDescription { get; }
        public string EventVenue { get; }

        public CalendarEvent(int eventId, int eventTypeId, string eventType, DateTime eventStart, DateTime eventEnd, string eventTitle, string eventDescription, string eventVenue)
        {
            EventId = eventId;
            EventTypeId = eventTypeId;
            EventType = EventType;
            EventStart = eventStart;
            EventEnd = eventEnd;
            EventTitle = eventTitle;
            EventDescription = eventDescription;
            EventVenue = eventVenue;
        }
    }


    public class MyCalendar : IEnumerable<CalendarEvent>
    {
        private List<CalendarEvent> mEvents;

        public MyCalendar(DateTime startDate, DateTime endDate, int? typeId=null)
        {
            mEvents = new List<CalendarEvent>();
            using (SqlConnection connection = DB.SqlConnection)
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    SqlParameter pTypeId = new SqlParameter("@typeId", SqlDbType.Int);

                    if (typeId != null)
                    {
                        command.CommandText = "Select * FROM Calendar JOIN EventTypes ON EventTypeId=id WHERE EventEnd>=@today AND EventEnd<@endDate AND EventTypeId=@typeId Order by EventStart;";
                        pTypeId.Value = typeId;
                        command.Parameters.Add(pTypeId);
                    }
                    else
                    {
                        command.CommandText = "Select * FROM Calendar JOIN EventTypes ON EventTypeId=id WHERE EventEnd>=@today AND EventEnd<@endDate Order by EventStart;";
                    }

                    SqlParameter today = new SqlParameter("@today", SqlDbType.DateTime2, 0);
                    today.Value = startDate;
                    command.Parameters.Add(today);

                    SqlParameter pEndDate = new SqlParameter("@endDate", SqlDbType.DateTime2, 0);
                    pEndDate.Value = endDate;
                    command.Parameters.Add(pEndDate);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            mEvents.Add(new CalendarEvent(
                                (int)reader["EventId"],
                                (int)reader["EventTypeId"],
                                (string)reader["EventType"],
                                (DateTime)reader["EventStart"],
                                (DateTime)reader["EventEnd"],
                                (string)reader["EventTitle"],
                                (string)reader["EventDescription"],
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
                                (DateTime)reader["EventStart"],
                                (DateTime)reader["EventEnd"],
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
            if (updatedEvent == null) return 0;

            using (SqlConnection connection = DB.SqlConnection)
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"UPDATE Calendar SET EventTypeId=@typeId, EventStart=@eventStart, EventEnd=@eventEnd, EventTitle=@eventTitle, EventDescription=@eventDescription, EventVenue=@eventVenue WHERE EventId=@id;";
                    command.Parameters.AddWithValue("@id", updatedEvent.EventId);
                    command.Parameters.AddWithValue("@typeId", updatedEvent.EventTypeId);
                    command.Parameters.AddWithValue("@eventStart", updatedEvent.EventStart);
                    command.Parameters.AddWithValue("@eventEnd", updatedEvent.EventEnd);
                    command.Parameters.AddWithValue("@eventTitle", updatedEvent.EventTitle);
                    command.Parameters.AddWithValue("@eventDescription", updatedEvent.EventDescription);
                    command.Parameters.AddWithValue("@eventVenue", updatedEvent.EventVenue);

                    connection.Open();
                    return command.ExecuteNonQuery();
                }
            }
        }

        public static int AddEvent(int typeId, DateTime fromDateTime, DateTime untilDateTime, string title, string description, string venue)
        {

            if (typeId < 0 || fromDateTime == null || untilDateTime == null || string.IsNullOrWhiteSpace(title))
                return 0;

            using (SqlConnection connection = DB.SqlConnection)
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"INSERT INTO Calendar VALUES(@typeId, @eventStart, @eventEnd, @eventTitle, @eventDescription, @eventVenue);";
                    command.Parameters.AddWithValue("@typeId", typeId);
                    command.Parameters.AddWithValue("@eventStart", fromDateTime);
                    command.Parameters.AddWithValue("@eventEnd", untilDateTime);
                    command.Parameters.AddWithValue("@eventTitle", title);
                    command.Parameters.AddWithValue("@eventDescription", description);
                    command.Parameters.AddWithValue("@eventVenue", venue);

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

