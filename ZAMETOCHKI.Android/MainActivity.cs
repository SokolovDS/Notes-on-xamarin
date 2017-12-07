using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using SQLite;
using System.Collections;
using System.Collections.Generic;
using static ZAMETOCHKI.Droid.SQLite;

namespace ZAMETOCHKI.Droid
{
	[Activity (Label = "ZAMETOCHKI.Android", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		int count = 1;

        protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			// Get our button from the layout resource,
			// and attach an event to it
			Button button = FindViewById<Button> (Resource.Id.myButton);
            Button DeleteButton = FindViewById<Button>(Resource.Id.DeleteAllButton);

            var AddButton = FindViewById<ImageButton>(Resource.Id.AddButton);


            LinearLayout llMain = (LinearLayout)FindViewById(Resource.Id.llMain);

            //Список всех заметок
            List<Memo> memos = SQLite.GetAllData();
            foreach (Memo memo in memos)
            {
                Button memoHeader = new Button(this);
                memoHeader.Text = memo.Header;
                llMain.AddView(memoHeader);
                Console.WriteLine(memo.Header);
            }

            button.Click += delegate {
				button.Text = string.Format ("{0} clicks!", count++);
            };

            DeleteButton.Click += delegate {
                DeleteAllMemos();
            };

            //Кнопка для создания заметки
            AddButton.Click += delegate
            {
                Intent intent = new Intent(this, typeof(MemoActivity));
                StartActivity(intent);
            };


        }
    }
}


