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
    class SQLite
    {
        [Table("Items")]
        public class Memo
        {
            [PrimaryKey, AutoIncrement, Column("_id")]
            public int Id { get; set; }
            [MaxLength(16)]  
            public string Header { get; set; }
            public string Text { get; set; }
        }

        public static void DoSomeDataAccess(String header, String text)
        {
            Console.WriteLine("Creating database, if it doesn't already exist");
            string dbPath = Path.Combine(
                 System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal),
                 "Memos.db3");
            var db = new SQLiteConnection(dbPath);
            db.CreateTable<Memo>();
            //if (db.Table<Stock>().Count() == 0)
            //{
            //    // only insert the data if it doesn't already exist
            //    var newStock = new Stock();
            //    newStock.Symbol = name;
            //    db.Insert(newStock);
            //    newStock = new Stock();
            //    newStock.Symbol = "GOOG";
            //    db.Insert(newStock);
            //    newStock = new Stock();
            //    newStock.Symbol = "MSFT";
            //    db.Insert(newStock);
            //}
            var newStock = new Memo();
            newStock.Header = header;
            newStock.Text = text;
            db.Insert(newStock);

            Console.WriteLine("Reading data");
            var table = db.Table<Memo>();
            foreach (var s in table)
            {
                Console.WriteLine(s.Id + " " + s.Header + " " + s.Text);
            }


        }
        public static List<Memo> GetAllData()
        {
            string dbPath = Path.Combine(
                 System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal),
                 "Memos.db3");
            var db = new SQLiteConnection(dbPath);
            var stock = db.Table<Memo>();

            //List<Memo> list = new List<Memo>();

            //list = (from p in db.Table<Memo>()
            //        select new Memo()
            //        {
            //            Header = p.Header,
            //            Text = p.Text
            //        })
            //.ToList();

            return db.Table<Memo>().ToList();
        }

        public static void DeleteMemo(Memo memo)
        {
            string dbPath = Path.Combine(
                 System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal),
                 "Memos.db3");
            var db = new SQLiteConnection(dbPath);

            db.Delete<Memo>(memo.Id);
        }

        public static void DeleteAllMemos()
        {
            string dbPath = Path.Combine(
                 System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal),
                 "Memos.db3");
            var db = new SQLiteConnection(dbPath);

            db.DeleteAll<Memo>();
        }
    }
}