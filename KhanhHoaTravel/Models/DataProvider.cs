using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace KhanhHoaTravel.Models
{
    public class DataProvider
    {
        static string connStr = "Server=DESKTOP-6OQ42LL;Database=KhanhHoaTraveler;Trusted_Connection=True;MultipleActiveResultSets=true ";
        static SqlConnection conn = new SqlConnection();
        static SqlCommand cmd = new SqlCommand();
        static SqlDataReader reader = null;
        public static void connectDb()
        {
            conn = new SqlConnection();
            conn.ConnectionString = connStr;
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            else
                return;
        }

        public static void setNullSession(HttpContext context, string key)
        {
            var session = context.Session;          // Lấy ISession
            string key_access = key;
            string json = session.GetString(key_access);
            json = null;
            string jsonSave = JsonConvert.SerializeObject(null);
            session.SetString(key_access, jsonSave);
        }

        public static void logout(HttpContext context)
        {
            var session = context.Session;
            string key_access = "UserLogin";
            setNullSession(context, key_access);
        }

        public static _User getUser(HttpContext context)
        {
            var session = context.Session;          // Lấy ISession
            string key_access = "UserLogin";
            string json = session.GetString(key_access);
            _User u = new _User();
            if (json != null) {
                // Convert chuỗi Json - thành đối tượng
                u = JsonConvert.DeserializeObject<_User>(json);
                u = getUserInfo(u);
            }
            string jsonSave = JsonConvert.SerializeObject(u);
            session.SetString(key_access, jsonSave);
            return u;
        }

        public static _User getUserInfo(_User user)
        {
            connectDb();
            cmd.Connection = conn;
            cmd.CommandText = "select * from UserDetail where Uid = (select Uid from VerifyUser where UserName = '" + user.UserName + "' and Password = '" + user.Password + "')";
            reader = cmd.ExecuteReader();
            if (reader.HasRows) {
                while (reader.Read()) {
                    user.Id = int.Parse(reader["Uid"].ToString());
                    user.DateCreate = reader["dateCreate"].ToString();
                    user.Email = reader["Email"].ToString();
                    user.FullName = reader["Name"].ToString();
                    user.FaceBook = reader["FaceBook"].ToString();
                    user.Phone = reader["Phone"].ToString();
                    user.Website = reader["Website"].ToString();
                }
            }
            reader.Close();

            cmd.CommandText = string.Format("select * from UserImage where Uid = {0} and status = 1", user.Id);
            reader = cmd.ExecuteReader();
            if (reader.HasRows) {
                while (reader.Read())
                    user.Image = reader["ImageLink"].ToString();
            }

            reader.Close();
            cmd.CommandText = string.Format("SELECT id AS 'Uid',RoleName AS 'Role' FROM dbo._User INNER JOIN dbo.Roles ON Roles.roleid = _User.roleid WHERE id = {0} AND STATUS = 1", user.Id);
            reader = cmd.ExecuteReader();
            if (reader.HasRows) {
                while (reader.Read())
                    user.Role = reader["Role"].ToString();

            }
            conn.Close();
            return user;
        }
        
        public static _User getUserInfoById(int uid) {
            connectDb();
            cmd.Connection = conn;
            cmd.CommandText = "select * from UserDetail where Uid = " + uid.ToString();
            reader = cmd.ExecuteReader();
            _User user = new _User();
            if (reader.HasRows) {
                while (reader.Read()) {
                    user.Id = int.Parse(reader["Uid"].ToString());
                    user.DateCreate = reader["dateCreate"].ToString();
                    user.Email = reader["Email"].ToString();
                    user.FullName = reader["Name"].ToString();
                    user.FaceBook = reader["FaceBook"].ToString();
                    user.Phone = reader["Phone"].ToString();
                    user.Website = reader["Website"].ToString();
                }
            }
            reader.Close();
            //Get User image
            cmd.CommandText = string.Format("select * from UserImage where Uid = {0} and status = 1", user.Id);
            reader = cmd.ExecuteReader();
            if (reader.HasRows) {
                while (reader.Read())
                    user.Image = reader["ImageLink"].ToString();
            }
            reader.Close();
            //Get user authen info
            cmd.CommandText = string.Format("select * from VerifyUser where Uid = {0}", user.Id);
            reader = cmd.ExecuteReader();
            if (reader.HasRows) {
                while (reader.Read()) {
                    user.UserName = reader["UserName"].ToString();
                    user.Password = reader["Password"].ToString();
                }
            }
            conn.Close();
            return user;
        }

        public static void getUserInfoById(int uid, _User user)
        {
            connectDb();
            cmd.Connection = conn;
            cmd.CommandText = "select * from UserDetail where Uid = " + uid.ToString();
            reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    user.DateCreate = reader["dateCreate"].ToString();
                    user.Email = reader["Email"].ToString();
                    user.FullName = reader["Name"].ToString();
                    user.FaceBook = reader["FaceBook"].ToString();
                    user.Phone = reader["Phone"].ToString();
                    user.Website = reader["Website"].ToString();
                }
            }
            reader.Close();
            cmd.CommandText = string.Format("select * from UserImage where Uid = {0} and status = 1", user.Id);
            reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    user.Image = reader["ImageLink"].ToString();
                }
            }
            conn.Close();
        }

        public static List<_User> getListUser()
        {
            List<_User> ul = new List<_User>();
            connectDb();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT * FROM dbo._User";

            reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    _User user = new _User();
                    user.Id = int.Parse(reader["id"].ToString());
                    int role = int.Parse(reader["roleid"].ToString());
                    if (role == 1)
                        user.Role = "admin";
                    else
                        user.Role = "user";
                    user.Status = int.Parse(reader["STATUS"].ToString());
                    ul.Add(user);
                }
            }
            reader.Close();
            conn.Close();

            foreach (var u in ul)
                getUserInfoById(u.Id, u);
            return ul;
        }

        public static void updateUserData(_User user)
        {
            connectDb();
            cmd.Connection = conn;
            cmd.CommandText = String.Format("UPDATE dbo.UserDetail SET Name = N'{0}',Email='{1}',Phone='{2}',FaceBook='{3}',Website='{4}' WHERE Uid = {5}",
                                                                    user.FullName,    user.Email, user.Phone, user.FaceBook, user.Website, user.Id);
            cmd.ExecuteNonQuery();
            if(user.Image != "")
            {
                cmd.CommandText = String.Format("UPDATE dbo.UserImage SET ImageLink = '/image/User/{0}' WHERE Uid = {1} ", user.Image, user.Id);
                cmd.ExecuteNonQuery();
            }
            conn.Close();
        }

        public static string getUserRole(int id)
        {
            connectDb();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT roleid FROM dbo._User where id = " + id;
            reader = cmd.ExecuteReader();
            if (reader.HasRows)
                while (reader.Read())
                    if (reader["roleid"].ToString() == "1")
                    {
                        conn.Close();
                        return "admin";
                    }
            conn.Close();
            return "user";
        }

        public static bool isUserExist(_User user)
        {
            connectDb();
            cmd.Connection = conn;
            cmd.CommandText = string.Format("SELECT Uid,UserName,Password,STATUS FROM dbo.VerifyUser INNER JOIN dbo._User ON _User.id = VerifyUser.Uid WHERE UserName = '{0}' AND Password = '{1}' AND STATUS = 1", user.UserName, user.Password);
            reader = cmd.ExecuteReader();
            if (reader.Read()) {
                conn.Close();
                return true;
            }
            else {
                conn.Close();
                return false;
            }
        }

        public static void changeStatusUser(int id, int status)
        {
            connectDb();
            cmd.Connection = conn;
            if (status == 1)
                cmd.CommandText = String.Format("UPDATE dbo._User SET STATUS = 0 WHERE id = {0}", id);
            else
                cmd.CommandText = String.Format("UPDATE dbo._User SET STATUS = 1 WHERE id = {0}", id);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        #region Post DataProvider
        public static void getImagePost(List<Post> lstPost)
        {
            connectDb();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT * FROM dbo.tblPostImage where status = 1";
            reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    int PostId = int.Parse(reader["PostId"].ToString());
                    String Path = reader["Path"].ToString();
                    foreach (var item in lstPost)
                        if (item.Id == PostId)
                            item.ImagePath = Path;
                }
            }
            conn.Close();
        }

        public static List<Post> getListPost()
        {
            List<Post> posts = new List<Post>();

            connectDb();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT * FROM dbo.tblPost where Status = 1";

            reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Post p = new Post();
                    p.Id = int.Parse(reader["id"].ToString());
                    p.Content = reader["Content"].ToString();
                    p.TimeModified = reader["TimeModified"].ToString();
                    _User u = new _User();
                    u.Id = int.Parse(reader["Uid"].ToString());
                    p.User = u;
                    p.status = int.Parse(reader["Status"].ToString());
                    connectDb();
                    posts.Insert(0, p);
                }
            }
            reader.Close();
            conn.Close();

            foreach (var p in posts)
                p.User = getUserInfoById(p.User.Id);
            getImagePost(posts);
            return posts;
        }

        public static List<Post> getAllPost()
        {
            List<Post> posts = new List<Post>();

            connectDb();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT * FROM dbo.tblPost";

            reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Post p = new Post();
                    p.Id = int.Parse(reader["id"].ToString());
                    p.Content = reader["Content"].ToString();
                    p.TimeModified = reader["TimeModified"].ToString();
                    _User u = new _User();
                    u.Id = int.Parse(reader["Uid"].ToString());
                    p.User = u;
                    p.status = int.Parse(reader["Status"].ToString());
                    connectDb();
                    posts.Insert(0, p);
                }
            }
            reader.Close();
            conn.Close();

            foreach (var p in posts)
                p.User = getUserInfoById(p.User.Id);
            getImagePost(posts);
            return posts;
        }

        public static List<PostLike> getPostLike()
        {
            List<PostLike> postLikes = new List<PostLike>();

            connectDb();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT * FROM dbo.PostLike";

            reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    PostLike pl = new PostLike();
                    pl.PostId = int.Parse(reader["Pid"].ToString());
                    pl.Userid = int.Parse(reader["Uid"].ToString());
                    postLikes.Add(pl);
                }
            }
            reader.Close();
            conn.Close();
            return postLikes;
        }

        public static List<PostComment> getPostComment()
        {
            List<PostComment> postComments = new List<PostComment>();

            connectDb();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT * FROM dbo.PostComment where Status = 1";

            reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    PostComment pc = new PostComment();
                    pc.Id = int.Parse(reader["id"].ToString());
                    pc.PostId = int.Parse(reader["Pid"].ToString());
                    _User user = new _User();
                    user.Id = int.Parse(reader["Uid"].ToString());
                    pc.User = user;
                    pc.Content = reader["Content"].ToString();
                    pc.TimeModified = reader["TimeModified"].ToString();
                    pc.status = int.Parse(reader["Status"].ToString());
                    postComments.Add(pc);
                }
            }
            reader.Close();
            conn.Close();

            foreach(var pc in postComments)
            {
                pc.User = getUserInfoById(pc.User.Id);
            }
            return postComments;
        }
        public static int getMaxPostId()
        {
            int max= 0;
            connectDb();
            cmd.Connection = conn;

            cmd.CommandText = "SELECT MAX(id) as max FROM dbo.tblPost";
            reader = cmd.ExecuteReader();
            if (reader.HasRows) {
                while (reader.Read())
                    max = int.Parse(reader["max"].ToString());
            }
            reader.Close();
            conn.Close();

            return max;
        }

        public static void addPost(List<Post> lst, Post newPost)
        {
            int maxPostId = getMaxPostId() + 1;
            connectDb();
            cmd.Connection = conn;
            
            cmd.CommandText = String.Format("INSERT INTO dbo.tblPost VALUES ({0}, N'{1}',GETDATE(),1)", newPost.User.Id, newPost.Content);
            cmd.ExecuteNonQuery();
            if (!String.IsNullOrEmpty(newPost.ImagePath))
            {
                cmd.CommandText = String.Format("INSERT INTO dbo.tblPostImage VALUES ({0},'/image/Post/{1}',1)", maxPostId, newPost.ImagePath);
                cmd.ExecuteNonQuery();
            }

            lst.Insert(0, newPost);
            conn.Close();
        }
        public static void addComment(PostComment postComment) {
            connectDb();
            cmd.Connection = conn;
            cmd.CommandText = String.Format("INSERT INTO dbo.PostComment VALUES({0}, {1}, N'{2}', GETDATE(), 1)", postComment.User.Id, postComment.PostId, postComment.Content);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public static void changeStatusPost(int id, int status)
        {
            connectDb();
            cmd.Connection = conn;
            if (status == 1)
                cmd.CommandText = String.Format("UPDATE dbo.tblPost SET Status = 0 WHERE id = {0}", id);
            else
                cmd.CommandText = String.Format("UPDATE dbo.tblPost SET Status = 1 WHERE id = {0}", id);
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        #endregion

        #region Place Provider
        public static List<EntertainmentPlace> getListPlace()
        {
            List<EntertainmentPlace> places = new List<EntertainmentPlace>();
            connectDb();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT tblPlace.id,Title,Author,TimeOpen,TimeClose,Rate,Address,Genre,Description,ImagePath,tblPlace.status as state " + 
                                "FROM dbo.tblPlace, dbo.tblPlaceDeltail " +
                                "WHERE dbo.tblPlace.id = tblPlaceDeltail.Id";

            reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    EntertainmentPlace place = new EntertainmentPlace();
                    place.Id = int.Parse(reader["id"].ToString());
                    place.Title = reader["Title"].ToString();
                    place.Author = reader["Author"].ToString();
                    place.TimeClose = reader["TimeClose"].ToString();
                    place.TimeOpen = reader["TimeOpen"].ToString();
                    place.Rate = float.Parse(reader["Rate"].ToString());
                    place.Address = reader["Address"].ToString();
                    place.Genre = reader["Genre"].ToString();
                    place.Description = reader["Description"].ToString();
                    place.ImagePath = reader["ImagePath"].ToString();
                    place.Status = int.Parse(reader["state"].ToString());
                    places.Add(place);
                }
            }
            reader.Close();
            conn.Close();
            return places;
        }

        public static List<string> getListPlaceImage(int placeId)
        {
            List<string> lstImage = new List<string>();
            connectDb();
            cmd.Connection = conn;
            cmd.CommandText = string.Format("SELECT Path FROM dbo.PlaceImage WHERE PlaceId = {0} AND status = 1", placeId);

            reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    string imgPath = reader["Path"].ToString();
                    lstImage.Add(imgPath);
                }
            }
            reader.Close();
            conn.Close();

            return lstImage;
        }

        public static EntertainmentPlace getPlaceDetail(int placeId)
        {
            EntertainmentPlace place = new EntertainmentPlace();
            connectDb();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT * FROM dbo.tblPlaceDeltail where id = " + placeId;

            reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    place.Id = int.Parse(reader["id"].ToString());
                    place.Title = reader["Title"].ToString();
                    place.Author = reader["Author"].ToString();
                    place.TimeClose = reader["TimeClose"].ToString();
                    place.TimeOpen = reader["TimeOpen"].ToString();
                    place.Rate = float.Parse(reader["Rate"].ToString());
                    place.Address = reader["Address"].ToString();
                    place.Genre = reader["Genre"].ToString();
                    place.Description = reader["Description"].ToString();
                    place.ImagePath = reader["ImagePath"].ToString();
                }
            }
            reader.Close();
            conn.Close();
            return place;
        }

        public static void changeStatusPlace(int id, int status)
        {
            connectDb();
            cmd.Connection = conn;
            if (status == 1) {
                cmd.CommandText = String.Format("UPDATE dbo.tblPlaceDeltail SET status = 0 WHERE Id = {0}", id);
                cmd.ExecuteNonQuery();
                cmd.CommandText = String.Format("UPDATE dbo.tblPlace SET status = 0 WHERE Id = {0}", id);
                cmd.ExecuteNonQuery();
            }
            else {
                cmd.CommandText = String.Format("UPDATE dbo.tblPlaceDeltail SET status = 1 WHERE Id = {0}", id);
                cmd.ExecuteNonQuery();
                cmd.CommandText = String.Format("UPDATE dbo.tblPlace SET status = 1 WHERE Id = {0}", id);
                cmd.ExecuteNonQuery();
            }
                
            conn.Close();
        }
        #endregion

    }
}
