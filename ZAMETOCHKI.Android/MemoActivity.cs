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
using static ZAMETOCHKI.Droid.SQLite;

namespace ZAMETOCHKI.Droid
{
    [Activity(Label = "MemoActivity", NoHistory = true)]
    public class MemoActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Memo);
            //Кнопка сохранения
            Button SaveButton = FindViewById<Button>(Resource.Id.SaveButton);
            //Заголовок
            var NoteHeader = FindViewById<EditText>(Resource.Id.NoteHeader);
            //Текст
            var NoteText = FindViewById<EditText>(Resource.Id.NoteText);

            
            string header = "";
            string text="";

            int MemoID = Intent.GetIntExtra("MemoID", -1);
            bool toEdit = Intent.GetBooleanExtra("isCreated", false);

            

            if (toEdit)
            {
                Memo CurMemo = GetData(MemoID);
                header = CurMemo.Header;
                text = CurMemo.Text;
                NoteHeader.Text = CurMemo.Header;
                NoteText.Text = CurMemo.Text;
                //Действие при сохранении
                SaveButton.Click += delegate
                {
                    ZAMETOCHKI.Droid.SQLite.EditMemo(CurMemo.Id, header, text);
                    Intent iintent = new Intent(this, typeof(MainActivity));
                    StartActivity(iintent);
                };
            }
            else
            {
                //Действие при сохранении
                SaveButton.Click += delegate
                {
                    ZAMETOCHKI.Droid.SQLite.CreateMemo(header, text);
                    Intent iintent = new Intent(this, typeof(MainActivity));
                    StartActivity(iintent);
                };
            }


            //Изменение заголовка заметки
            NoteHeader.TextChanged += (object sender, Android.Text.TextChangedEventArgs e) => {
                header = e.Text.ToString();
            };

            //Изменение текста заметки
            NoteText.TextChanged += (object sender, Android.Text.TextChangedEventArgs e) => {
                text = e.Text.ToString();
            };
        }
    }
}