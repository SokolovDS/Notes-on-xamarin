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
        string header = "";
        string text = "";

        ActionMenuView amvMenu;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Memo);
            //Заголовок
            var NoteHeader = FindViewById<EditText>(Resource.Id.NoteHeader);
            //Текст
            var NoteText = FindViewById<EditText>(Resource.Id.NoteText);

            int MemoID = Intent.GetIntExtra("MemoID", -1);
            bool toEdit = Intent.GetBooleanExtra("isCreated", false);

            if (toEdit)
            {
                Memo CurMemo = GetData(MemoID);
                header = CurMemo.Header;
                text = CurMemo.Text;
                NoteHeader.Text = CurMemo.Header;
                NoteText.Text = CurMemo.Text;
            }

            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetActionBar(toolbar);
            ActionBar.Title = "";


            //Изменение заголовка заметки
            NoteHeader.TextChanged += (object sender, Android.Text.TextChangedEventArgs e) =>
            {
                header = e.Text.ToString();
            };

            //Изменение текста заметки
            NoteText.TextChanged += (object sender, Android.Text.TextChangedEventArgs e) =>
            {
                text = e.Text.ToString();
            };
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.memo_menu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            Toast.MakeText(this, "Action selected: " + item.TitleFormatted,
                ToastLength.Short).Show();

            int MemoID = Intent.GetIntExtra("MemoID", -1);
            bool toEdit = Intent.GetBooleanExtra("isCreated", false);

            if (toEdit)
            {
                    ZAMETOCHKI.Droid.SQLite.EditMemo(MemoID, header, text);
                    Finish();
            }
            else
            {
                    int id = ZAMETOCHKI.Droid.SQLite.CreateMemo(header, text);
                    Intent intent = new Intent();
                    intent.PutExtra("id", id);
                    SetResult((Result)1, intent);
                    Finish();
            }

            return base.OnOptionsItemSelected(item);
        }
    }
}