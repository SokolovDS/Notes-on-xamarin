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
using Android.Graphics;

namespace ZAMETOCHKI.Droid
{

	[Activity (Label = "ZAMETOCHKI.Android", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
        const int REQUEST_ID = 1;
        int spinnerPos = -1;

        public class MemoButton
        {
            public string Header { get; set; }
            public string Date { get; set; }
        }
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            
            SetContentView(Resource.Layout.Main);


            LinearLayout llMain = (LinearLayout)FindViewById(Resource.Id.llMain);

            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetActionBar(toolbar);
            ActionBar.Title = "Заметки";


            //Получаем список всех элементов
            List<Memo> memos = SQLite.GetAllData();

            //Удаление всех заметок
            ////DeleteButton.Click += delegate
            ////{
            ////    DeleteAllMemos();
            ////    llMain.RemoveViews(0, llMain.ChildCount);
            ////};
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

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.main_menu, menu);



            var item = menu.FindItem(Resource.Id.menu_spinner);
            Spinner sortType = (Spinner)item.ActionView;

            //Спиннер сортировки
            sortType.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);
            var adapter = ArrayAdapter.CreateFromResource(
                    this, Resource.Array.sortList, Resource.Layout.spinner_item);

            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            sortType.Adapter = adapter;

            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            LinearLayout llMain = (LinearLayout)FindViewById(Resource.Id.llMain);

            switch (item.ItemId)
            {
                case Resource.Id.menu_create:
                    Intent intent = new Intent(this, typeof(MemoActivity));
                    //StartActivityForResult(intent, REQUEST_ID);
                    StartActivity(intent);

                    Toast.MakeText(this, "Action selected: " + item.TitleFormatted,
                        ToastLength.Short).Show();
                    return base.OnOptionsItemSelected(item);
                case Resource.Id.menu_delete:
                    DeleteAllMemos();
                    llMain.RemoveViews(0, llMain.ChildCount);
                    return base.OnOptionsItemSelected(item);
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        //protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        //{
        //    base.OnActivityResult(requestCode, resultCode, data);
        //    if (resultCode == (Result)1)
        //        switch (requestCode)
        //        {
        //            case REQUEST_ID:
        //                int id = data.GetIntExtra("id", -1);
        //                if (id != -1) printMemo(id);
        //                break;
        //        }
        //}

        //Добавление кнопки заметки
        protected void printMemo(int id)
        {
            LinearLayout llMain = (LinearLayout)FindViewById(Resource.Id.llMain);

            var memo = SQLite.GetData(id);

            Button memoHeader = new Button(this);
            memoHeader.Text = memo.Header+"\n"+memo.CreationTime.ToString();
            memoHeader.Click += delegate
            {
                Intent intent = new Intent(this, typeof(MemoActivity));
                intent.PutExtra("MemoID", id);
                intent.PutExtra("isCreated", true);
                StartActivity(intent);
            };
            llMain.AddView(memoHeader);

            //TextView memoHeader = new TextView(this, null, Resource.Style.CustomTextView);
            //memoHeader.Text = memo.Header;

            //TextView memoText = new TextView(this);
            //memoText.Text = memo.Text;

            //TextView memoDate = new TextView(this);
            //memoDate.Text = memo.CreationTime.ToString();

            //LinearLayout memoll = new LinearLayout(this, null, Resource.Style.MyTheme);
            //memoll.AddView(memoHeader);
            //memoll.AddView(memoText);
            //memoll.AddView(memoDate);

            //memoll.Click += delegate
            //{
            //    Intent intent = new Intent(this, typeof(MemoActivity));
            //    intent.PutExtra("MemoID", id);
            //    intent.PutExtra("isCreated", true);
            //    StartActivity(intent);
            //};


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


