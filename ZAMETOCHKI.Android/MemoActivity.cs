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
    [Activity(Label = "MemoActivity")]
    public class MemoActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Memo);
            Button SaveButton = FindViewById<Button>(Resource.Id.SaveButton);
            var NoteHeader = FindViewById<EditText>(Resource.Id.NoteHeader);
            var NoteText = FindViewById<EditText>(Resource.Id.NoteText);



            // Create your application here
            string header = "";
            string text="";

            NoteHeader.TextChanged += (object sender, Android.Text.TextChangedEventArgs e) => {

                header = e.Text.ToString();

            };

            NoteText.TextChanged += (object sender, Android.Text.TextChangedEventArgs e) => {

                text = e.Text.ToString();

            };

            SaveButton.Click += delegate
            {
                DoSomeDataAccess(header, text);
                Intent intent = new Intent(this, typeof(MainActivity));
                StartActivity(intent);
            };
        }
    }
}