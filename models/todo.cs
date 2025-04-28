using System.Dynamic;
using System.Security.Cryptography;
using MySql.Data.MySqlClient;

namespace models.ToDoList;
public class ToDoList
{

    public static async Task<IResult> GetTodoLists(ExpandoObject body, MySqlConnection connection, HttpContext context)
    {
        try
        {
            // หากบันทึกไฟล์สำเร็จ บันทึก Metadata ในฐานข้อมูล
            string sql = "SELECT * FROM todolist";
            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                 using (MySqlDataReader reader = command.ExecuteReader())
                {
                    var todolists = new List<object>();
                    while (reader.Read())
                    {
                        var todolist = new
                        {
                            todolistId = reader.GetString("todolistId"),
                            todolist = reader.GetString("todolist"),
                            isCheck = reader.GetInt32("isCheck"),
                        };
                        todolists.Add(todolist);
                       
                    }
                        return Results.Ok(todolists); // Return the first result
                    
                }
            }
        }
        catch (Exception ex)
        {
            // หากเกิดข้อผิดพลาดใน SQL
            // ลบไฟล์ออกจาก File System เพื่อป้องกันไฟล์ตกค้าง
            // _fileService.DeleteImageFromFileSystem(fileName, _uploadPath);
            Console.WriteLine($"Error saving data to database product: {ex.Message}");
            // context.Response.StatusCode = 500; // ส่งสถานะ 500 กลับไป
            // context.Response.WriteAsync($"Error saving data to database: {ex.Message}");
            return Results.StatusCode(500); // หยุดการทำงานของฟังก์ชัน
        }
    }
    public static async Task<IResult> InsertTodo(ExpandoObject body, MySqlConnection connection, HttpContext context)
    {
        dynamic data = body;
        var todolist = data.todolist;
        var isCheck = data.isCheck;
        byte[] randomBytes = new byte[25];
        RandomNumberGenerator.Fill(randomBytes);
        string todolistId = BitConverter.ToString(randomBytes).Replace("-", "");
        DateTime currentDateTime = DateTime.Now;
        try
        {
            // หากบันทึกไฟล์สำเร็จ บันทึก Metadata ในฐานข้อมูล
            string sql = "INSERT INTO todolist (todolistId,todolist,isCheck,updateTime) VALUES (@todolistId,@todolist, @isCheck,@updateTime)";
            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@todolistId", todolistId);
                command.Parameters.AddWithValue("@todolist", todolist);
                command.Parameters.AddWithValue("@isCheck", isCheck);
                command.Parameters.AddWithValue("@updateTime", currentDateTime);
                command.ExecuteNonQuery();
            }
            return Results.Ok();
        }
        catch (Exception ex)
        {
            // หากเกิดข้อผิดพลาดใน SQL
            // ลบไฟล์ออกจาก File System เพื่อป้องกันไฟล์ตกค้าง
            // _fileService.DeleteImageFromFileSystem(fileName, _uploadPath);
            Console.WriteLine($"Error saving data to database product: {ex.Message}");
            // context.Response.StatusCode = 500; // ส่งสถานะ 500 กลับไป
            // context.Response.WriteAsync($"Error saving data to database: {ex.Message}");
            return Results.StatusCode(500); // หยุดการทำงานของฟังก์ชัน
        }
    }

    
}


public class ToDoListData{
    public string todolistId{get;set;}
    public string todolist{get;set;}
    public int isCheck{get;set;}
}