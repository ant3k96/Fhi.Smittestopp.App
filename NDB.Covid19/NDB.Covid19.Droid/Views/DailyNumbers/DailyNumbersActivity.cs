﻿using System;
using Android.App;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Support.V4.Content;
using Android.Text;
using Android.Text.Method;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.Core.Text;
using NDB.Covid19.Droid.Utils;
using NDB.Covid19.Droid.Views.InfectionStatus;
using NDB.Covid19.ViewModels;
using static NDB.Covid19.ViewModels.DailyNumbersViewModel;

namespace NDB.Covid19.Droid.Views.DailyNumbers
{
    [Activity(
        Theme = "@style/AppTheme",
        ParentActivity = typeof(InfectionStatusActivity),
        ScreenOrientation = ScreenOrientation.FullUser, LaunchMode = LaunchMode.SingleTop)]
    public class DailyNumbersActivity : AppCompatActivity
    {
        private static readonly DailyNumbersViewModel ViewModel;
        private ImageView _closeButton;

        static DailyNumbersActivity()
        {
            ViewModel = new DailyNumbersViewModel();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Title = DAILY_NUMBERS_HEADER;
            SetContentView(Resource.Layout.activity_daily_numbers);
            Init();
        }

        private void Init()
        {
            View rootView = Window.DecorView.RootView;
            rootView.LayoutDirection = LayoutUtils.GetLayoutDirection();
            UpdateUI();
            _closeButton = FindViewById<ImageView>(Resource.Id.daily_numbers_back_button);
            _closeButton.Click += new StressUtils.SingleClick(OnCloseBtnClicked).Run;
            _closeButton.ContentDescription = BACK_BUTTON_ACCESSIBILITY_TEXT;
            _closeButton.SetBackgroundResource(LayoutUtils.GetBackArrow());
        }

        private void AdjustLineBreaks(TextView viewA, TextView viewB)
        {
            if (viewA.LineCount > viewB.LineCount)
            {
                string addition = "";
                for (int i = 0; i < viewA.LineCount - viewB.LineCount; ++i)
                {
                    addition += "\n";
                }

                viewB.Text = viewB.Text + addition;
            }
        }

        private void AdjustLines(TextView viewA, TextView viewB)
        {
            AdjustLineBreaks(viewA, viewB);
            AdjustLineBreaks(viewB, viewA);
        }
        
        private void UpdateUI()
        {
            TextView _dailyNumbersHeader = FindViewById<TextView>(Resource.Id.daily_numbers_header_textView);
            _dailyNumbersHeader.Text = DAILY_NUMBERS_HEADER;
            TextView _dailyNumbersSubHeaderStatistics = FindViewById<TextView>(Resource.Id.daily_numbers_statistics_header_textView);
            _dailyNumbersSubHeaderStatistics.Text = DAILY_NUMBERS_TITLE_ONE;
            _dailyNumbersSubHeaderStatistics.TextAlignment = TextAlignment.ViewStart;
            TextView _dailyNumbersSubTextStatistics = FindViewById<TextView>(Resource.Id.daily_numbers_statistics_text_textview);
            TextView _dailyNumbersSubTextSmittestopp = FindViewById<TextView>(Resource.Id.daily_numbers_smittestopp_text_textview);
            _dailyNumbersSubTextSmittestopp.TextAlignment = TextAlignment.ViewStart;
            SetupSubTextWithLink(_dailyNumbersSubTextStatistics, LastUpdateStringSubHeader);
            SetupSubTextWithLink(_dailyNumbersSubTextSmittestopp, LastUpdateStringSubSubHeader);

            FindViewById<TextView>(Resource.Id.daily_numbers_infected_header_text).Text = KEY_FEATURE_ONE_LABEL;
            FindViewById<TextView>(Resource.Id.daily_numbers_infected_number_text).Text = ConfirmedCasesToday;
            FindViewById<TextView>(Resource.Id.daily_numbers_infected_total_text).Text = ConfirmedCasesTotal;

            FindViewById<TextView>(Resource.Id.daily_numbers_tested_header_text).Text = KEY_FEATURE_THREE_LABEL;
            FindViewById<TextView>(Resource.Id.daily_numbers_tested_number_text).Text = TestsConductedToday;
            FindViewById<TextView>(Resource.Id.daily_numbers_tested_total_text).Text = TestsConductedTotal;

            FindViewById<TextView>(Resource.Id.daily_numbers_hospitalized_header_text).Text = KEY_FEATURE_FOUR_LABEL;
            FindViewById<TextView>(Resource.Id.daily_numbers_hospitalized_number_text).Text = PatientsAdmittedToday;

            FindViewById<TextView>(Resource.Id.daily_numbers_intensive_header_text).Text = KEY_FEATURE_SEVEN_LABEL;
            FindViewById<TextView>(Resource.Id.daily_numbers_intensive_number_text).Text = PatientsIntensiveCare;

            TextView _dailyNumbersSubHeaderVaccinations = FindViewById<TextView>(Resource.Id.daily_numbers_vaccinations_header_textView);
            _dailyNumbersSubHeaderVaccinations.Text = DAILY_NUMBERS_TITLE_TWO;

            TextView _dailyNumbersSubTextVaccinations = FindViewById<TextView>(Resource.Id.daily_numbers_vaccinations_text_textview);
            SetupSubTextWithLink(_dailyNumbersSubTextVaccinations, LastUpdateStringSubTextTwo);

            TextView keyFeatureNineTextView = FindViewById<TextView>(Resource.Id.daily_numbers_vaccination_dose1_header_text);
            keyFeatureNineTextView.Text = KEY_FEATURE_NINE_LABEL;
            keyFeatureNineTextView.ContentDescription = KEY_FEATURE_NINE_ACCESSIBILITY_LABEL;
            FindViewById<TextView>(Resource.Id.daily_numbers_vaccination_dose1_number_text).Text = VaccinationsDoseOneToday;
            FindViewById<TextView>(Resource.Id.daily_numbers_vaccination_dose1_total_text).Text = VaccinationsDoseOneTotal;

            TextView keyFeatureTenTextView = FindViewById<TextView>(Resource.Id.daily_numbers_vaccination_dose2_header_text);
            keyFeatureTenTextView.Text = KEY_FEATURE_TEN_LABEL;
            keyFeatureTenTextView.ContentDescription = KEY_FEATURE_TEN_ACCESSIBILITY_LABEL;
            FindViewById<TextView>(Resource.Id.daily_numbers_vaccination_dose2_number_text).Text = VaccinationsDoseTwoToday;
            FindViewById<TextView>(Resource.Id.daily_numbers_vaccination_dose2_total_text).Text = VaccinationsDoseTwoTotal;

            TextView _dailyNumbersSubHeaderSmittestopp = FindViewById<TextView>(Resource.Id.daily_numbers_smittestopp_header_textView);
            _dailyNumbersSubHeaderSmittestopp.Text = DAILY_NUMBERS_TITLE_THREE;
            _dailyNumbersSubHeaderSmittestopp.TextAlignment = TextAlignment.ViewStart;

            //Added newline for the UI to align.
            FindViewById<TextView>(Resource.Id.daily_numbers_smittestopp_downloads_header_text).Text = $"{KEY_FEATURE_SIX_LABEL} \n";
            FindViewById<TextView>(Resource.Id.daily_numbers_smittestopp_downloads_number_text).Text = SmittestopDownloadsTotal;

            FindViewById<TextView>(Resource.Id.daily_numbers_positive_shared_header_text).Text = KEY_FEATURE_FIVE_LABEL;
            FindViewById<TextView>(Resource.Id.daily_numbers_positive_shared_number_text).Text = NumberOfPositiveTestsResultsLast7Days;
            FindViewById<TextView>(Resource.Id.daily_numbers_positive_shared_total_text).Text = NumberOfPositiveTestsResultsTotal;
            
            // CONTENT DESCRIPTIONS OF HEADERS
            _dailyNumbersHeader.ContentDescription = DAILY_NUMBERS_HEADER.ToLower();
            _dailyNumbersSubHeaderStatistics.ContentDescription = DAILY_NUMBERS_TITLE_ONE.ToLower();
            _dailyNumbersSubHeaderVaccinations.ContentDescription = DAILY_NUMBERS_TITLE_TWO.ToLower();
            _dailyNumbersSubHeaderSmittestopp.ContentDescription = DAILY_NUMBERS_TITLE_THREE.ToLower();
            _dailyNumbersHeader.SetAccessibilityDelegate(AccessibilityUtils.GetHeadingAccessibilityDelegate());
            _dailyNumbersSubHeaderStatistics.SetAccessibilityDelegate(AccessibilityUtils.GetHeadingAccessibilityDelegate());
            _dailyNumbersSubHeaderVaccinations.SetAccessibilityDelegate(AccessibilityUtils.GetHeadingAccessibilityDelegate());
            _dailyNumbersSubHeaderSmittestopp.SetAccessibilityDelegate(AccessibilityUtils.GetHeadingAccessibilityDelegate());
        }

        public override void OnWindowFocusChanged(bool hasFocus)
        {
            base.OnWindowFocusChanged(hasFocus);
            if (hasFocus)
            {
                AdjustLines(
                   FindViewById<TextView>(Resource.Id.daily_numbers_infected_header_text),
                   FindViewById<TextView>(Resource.Id.daily_numbers_tested_header_text));
                AdjustLines(
                    FindViewById<TextView>(Resource.Id.daily_numbers_hospitalized_header_text),
                    FindViewById<TextView>(Resource.Id.daily_numbers_intensive_header_text));
                AdjustLines(
                    FindViewById<TextView>(Resource.Id.daily_numbers_vaccination_dose1_header_text),
                    FindViewById<TextView>(Resource.Id.daily_numbers_vaccination_dose2_header_text));
                AdjustLines(
                    FindViewById<TextView>(Resource.Id.daily_numbers_smittestopp_downloads_header_text),
                    FindViewById<TextView>(Resource.Id.daily_numbers_positive_shared_header_text));
            }
        }

        protected override void OnResume()
        {
            base.OnResume();
            RequestFHIDataUpdate(() => RunOnUiThread(UpdateUI));
        }

        private void OnCloseBtnClicked(object arg1, EventArgs arg2)
        {
            GoToInfectionStatusActivity();
        }

        public override void OnBackPressed() => Finish();

        private void GoToInfectionStatusActivity() => Finish();

        private void SetupSubTextWithLink (TextView textView, string formattedText)
        {
            ISpanned formattedDescription = HtmlCompat.FromHtml(formattedText, HtmlCompat.FromHtmlModeLegacy);
            textView.TextFormatted = formattedDescription;
            textView.ContentDescriptionFormatted = formattedDescription;
            textView.MovementMethod = LinkMovementMethod.Instance;
            Color linkColor = new Color(ContextCompat.GetColor(this, Resource.Color.linkColor));
            textView.SetLinkTextColor(linkColor);
        }

    }
}