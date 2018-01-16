using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;
using System.IO;

namespace ZAMETOCHKI.Droid
{
    static class SQLite
    {
        [Table("Items")]
        public class Memo
        {
            [PrimaryKey, AutoIncrement, Column("_id")]
            public int Id { get; set; }
            [MaxLength(16)]  
            public string Header { get; set; }
            public string Text { get; set; }
            public DateTime CreationTime { get; set; }

        }

        private static string _path = Path.Combine(
                 System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal),
                 "Memos.db3");

        private static SQLiteConnection _connection;

        static SQLite()
        {
            _connection = new SQLiteConnection(_path);
        }

        //Создание заметки
        public static int CreateMemo(String header, String text)
        {
            _connection.CreateTable<Memo>();
            var newStock = new Memo();
            newStock.Header = header;
            newStock.Text = text;
            newStock.CreationTime = DateTime.UtcNow;
            _connection.Insert(newStock);
            return newStock.Id;

        }

        //Изменение заметки
        public static void EditMemo(int id, String header, String text)
        {
            _connection.CreateTable<Memo>();
            var editMemo = _connection.Table<Memo>().FirstOrDefault(e => e.Id == id);
            editMemo.Header = header;
            editMemo.Text = text;
            editMemo.CreationTime = DateTime.UtcNow;
            _connection.Update(editMemo);
        }
        //Получение списка всех заметок
        public static List<Memo> GetAllData()
        {
            _connection.CreateTable<Memo>();
            var stock = _connection.Table<Memo>();
            return _connection.Table<Memo>().ToList();
        }

        //Получение списка всех заметок
        public static List<Memo> GetAllDataSorted(int typeofsort)
        {
            _connection.CreateTable<Memo>();
            var stock = _connection.Table<Memo>();
            var list = _connection.Table<Memo>().ToList();
            switch (typeofsort)
            {
                case 0:
                    list.Sort(delegate (Memo us1, Memo us2)
                    { return us2.CreationTime.CompareTo(us1.CreationTime); });
                    break;
                case 1:
                    list.Sort(delegate (Memo us1, Memo us2)
                    { return us1.Header.CompareTo(us2.Header); });
                    break;
                default: break;
            }
            return list;
        }

        //Получение данных одной заметки
        public static Memo GetData(int id)
        {
            _connection.CreateTable<Memo>();
            return _connection.Table<Memo>().FirstOrDefault(e => e.Id == id);
        }

        //Удаление заметки
        public static void DeleteMemo(int Id)
        {
            _connection.Delete<Memo>(Id);
        }

        //Удаление всех заметок
        public static void DeleteAllMemos()
        {
            _connection.DeleteAll<Memo>();
        }
    }
}