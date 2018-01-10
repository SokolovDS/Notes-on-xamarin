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
        const int REQUEST_ID = 1;
        int spinnerPos = -1;
        int typeOfSort = -1;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            
            SetContentView(Resource.Layout.Main);

            Spinner sortType = FindViewById<Spinner>(Resource.Id.sortType);
            Button DeleteButton = FindViewById<Button>(Resource.Id.DeleteAllButton);
            var AddButton = FindViewById<ImageButton>(Resource.Id.AddButton);

            LinearLayout llMain = (LinearLayout)FindViewById(Resource.Id.llMain);

            //Получаем список всех элементов
            List<Memo> memos = SQLite.GetAllData();

            //Удаление всех заметок
            DeleteButton.Click += delegate
            {
                DeleteAllMemos();
                llMain.RemoveViews(0, llMain.ChildCount);
            };

            //Переход к созданию заметки
            AddButton.Click += delegate
            {
                Intent intent = new Intent(this, typeof(MemoActivity));             
                StartActivityForResult(intent, REQUEST_ID);
            };

            //Спиннер сортировки
            sortType.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);
            var adapter = ArrayAdapter.CreateFromResource(
                    this, Resource.Array.sortType, Android.Resource.Layout.SimpleSpinnerDropDownItem);

            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            sortType.Adapter = adapter;
        }

        protected override void OnStart()
        {
            base.OnStart();

            LinearLayout llMain = (LinearLayout)FindViewById(Resource.Id.llMain);

            //Удаление всех кнопок заметок
            llMain.RemoveViews(0, llMain.ChildCount);
            //Получение списка заметок
            var memos = SQLite.GetAllDataSorted(spinnerPos);
            //Вывод их
            foreach (Memo memo in memos)
            {
                printMemo(memo.Id);
            }
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (resultCode == (Result)1)
                switch (requestCode)
                {
                    case REQUEST_ID:
                        int id = data.GetIntExtra("id", -1);
                        if (id != -1) printMemo(id);
                        break;
                }
        }

        //Добавление кнопки заметки
        protected void printMemo(int id)
        {
            LinearLayout llMain = (LinearLayout)FindViewById(Resource.Id.llMain);

            var memo = SQLite.GetData(id);
            Button memoHeader = new Button(this);
            memoHeader.Text = memo.Header;
            memoHeader.Click += delegate
            {
                Intent intent = new Intent(this, typeof(MemoActivity));
                intent.PutExtra("MemoID", id);
                intent.PutExtra("isCreated", true);
                StartActivity(intent);
            };
            llMain.AddView(memoHeader);
        }

        private void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;
            string toast = string.Format("Соритровка {0}", spinner.GetItemAtPosition(e.Position));
            Toast.MakeText(this, toast, ToastLength.Long).Show();
            spinnerPos = (int)e.Id;

            LinearLayout llMain = (LinearLayout)FindViewById(Resource.Id.llMain);

            //Удаление всех кнопок заметок
            llMain.RemoveViews(0, llMain.ChildCount);
            //Получение списка заметок
            var memos = SQLite.GetAllDataSorted(spinnerPos);
            //Вывод их
            foreach (Memo memo in memos)
            {
                printMemo(memo.Id);
            }
        }
    }
}


