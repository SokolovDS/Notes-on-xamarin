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
	[Activity (Label = "ZAMETOCHKI.Android", MainLauncher = true, Icon = "@drawable/icon", NoHistory = true)]
	public class MainActivity : Activity
	{
		int count = 1;

        protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.Main);

            Button button = FindViewById<Button> (Resource.Id.myButton);
            Button DeleteButton = FindViewById<Button>(Resource.Id.DeleteAllButton);
            
            var AddButton = FindViewById<ImageButton>(Resource.Id.AddButton);
            
            LinearLayout llMain = (LinearLayout)FindViewById(Resource.Id.llMain);

            //Получаем список всех элементов
            List<Memo> memos = SQLite.GetAllData();

            //Сортировка по дате создания
            memos.Sort(delegate (Memo us1, Memo us2)
            { return us2.CreationTime.CompareTo(us1.CreationTime); });

            //Вывод их
            foreach (Memo memo in memos)
            {
                Button memoHeader = new Button(this);
                memoHeader.Text = memo.Header;
                memoHeader.Click += delegate
                {
                    Intent intent = new Intent(this, typeof(MemoActivity));
                    intent.PutExtra("MemoID", memo.Id);
                    intent.PutExtra("isCreated", true);
                    StartActivity(intent);
                };
                llMain.AddView(memoHeader);
                Console.WriteLine(memo.Header);
            }

            button.Click += delegate {
				button.Text = string.Format ("{0} clicks!", count++);
            };

            //Удаление всех заметок
            DeleteButton.Click += delegate {
                DeleteAllMemos();
                llMain.RemoveViews(0, llMain.ChildCount);
            };

            //Переход к созданию заметки
            AddButton.Click += delegate
            {
                Intent intent = new Intent(this, typeof(MemoActivity));
                StartActivity(intent);
            };


        }
        
    }
}


