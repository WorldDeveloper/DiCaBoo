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
    public class DiaryPost
    {
        public int ID { get; set; }
        public DateTime DateTime { get; set; }
        public string Content { get; set; }
        public DiaryPost(int id, DateTime dateTime, string postContent)
        {
            ID = id;
            DateTime = dateTime;
            Content = postContent;
        }

    }

    public class Diary : IEnumerable<DiaryPost> //without generic??****************************
    {
        private List<DiaryPost> mPosts;

        public Diary()
        {
            Fill();
        }

        public void Fill()
        {
            mPosts = new List<DiaryPost>();
            using (SqlConnection connection = DB.SqlConnection)
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "Select * from Diary Order by PostDateTime Desc;";
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            mPosts.Add(new DiaryPost(
                                (int)reader["Id"],
                                (DateTime)reader["PostDateTime"],
                                (string)reader["Content"])
                                );
                        }
                    }
                }
            }
        }

        public static DiaryPost GetPost(int id)
        {
            using (SqlConnection connection = DB.SqlConnection)
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM Diary WHERE id=@id;";

                    SqlParameter idParam = new SqlParameter("@id", SqlDbType.Int);
                    idParam.Value = id;
                    command.Parameters.Add(idParam);

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                           return new DiaryPost(
                                (int)reader["Id"],
                                (DateTime)reader["PostDateTime"],
                                (string)reader["Content"]
                                );
                        }
                    }

                    return null;
                }
            }
        }

        public static int UpdatePost(int id, string postContent)
        {
            if (string.IsNullOrWhiteSpace(postContent)) return 0;

            using (SqlConnection connection = DB.SqlConnection)
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "UPDATE Diary SET Content=@PostContent WHERE id=@id;";

                    SqlParameter idParam = new SqlParameter("@id", SqlDbType.Int);
                    idParam.Value = id;
                    command.Parameters.Add(idParam);

                    SqlParameter content = new SqlParameter("@PostContent", SqlDbType.NVarChar, -1);
                    content.Value = postContent;
                    command.Parameters.Add(content);

                    connection.Open();
                    return command.ExecuteNonQuery();
                }
            }
        }

        public static int AddPost(string postContent)
        {
            if (string.IsNullOrWhiteSpace(postContent)) return 0;

            using (SqlConnection connection = DB.SqlConnection)
            {
                using (SqlCommand command=connection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO Diary VALUES(@DateTime, @PostContent);";

                    SqlParameter dateTime = new SqlParameter("@DateTime", SqlDbType.DateTime2, 0);
                    dateTime.Value = DateTime.Now;
                    command.Parameters.Add(dateTime);

                    SqlParameter content = new SqlParameter("@PostContent", SqlDbType.NVarChar, -1);
                    content.Value = postContent;
                    command.Parameters.Add(content);

                    connection.Open();
                    return  command.ExecuteNonQuery();
                }
            }
        }

        public static int RemovePost(int postId)
        {
            using (SqlConnection connection = DB.SqlConnection)
            {
                using (SqlCommand command=connection.CreateCommand())
                {
                    command.CommandText = "DELETE Diary WHERE id=@id;";

                    SqlParameter paramId = new SqlParameter("@id", SqlDbType.Int);
                    paramId.Value = postId;
                    command.Parameters.Add(paramId);

                    connection.Open();

                    return command.ExecuteNonQuery();
                }
            }
        }

        public IEnumerator<DiaryPost> GetEnumerator()
        {
            return mPosts.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return mPosts.GetEnumerator();
        }
    }
}
