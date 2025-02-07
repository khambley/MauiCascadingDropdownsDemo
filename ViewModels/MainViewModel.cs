using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using CascadingDropdownsDemo.Helpers;
using CascadingDropdownsDemo.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CascadingDropdownsDemo.ViewModels
{
	public partial class MainViewModel : INotifyPropertyChanged
    {
		public ObservableCollection<Category> Level1Categories { get; set; } = new();
        public ObservableCollection<Category> FilteredLevel1Categories { get; set; } = new();

        public ObservableCollection<Category> Level2Categories { get; set; } = new();
        public ObservableCollection<Category> FilteredLevel2Categories { get; set; } = new();

        public ObservableCollection<Category> Level3Categories { get; set; } = new();
        public ObservableCollection<Category> FilteredLevel3Categories { get; set; } = new();

        public ObservableCollection<Category> Level4Categories { get; set; } = new();
        public ObservableCollection<Category> FilteredLevel4Categories { get; set; } = new();

        #region Selection Properties
        private int selectedCategoryId;
        public int SelectedCategoryId
        {
            get => selectedCategoryId;
            private set
            {
                if(selectedCategoryId != value)
                {
                    selectedCategoryId = value;
                    OnPropertyChanged();
                }
            }
        }

        private Category selectedLevel1Category;
        public Category SelectedLevel1Category
        {
            get => selectedLevel1Category;
            set
            {
                if(selectedLevel1Category != value)
                {
                    selectedLevel1Category = value;
                    OnPropertyChanged();
                    UpdateLevel2Categories();
                    UpdateSelectedCategoryId();
                    IsLevel2Visible = selectedLevel1Category != null;
                }
            }
        }

        private Category selectedLevel2Category;
        public Category SelectedLevel2Category
        {
            get => selectedLevel2Category;
            set
            {
                if(selectedLevel2Category != value)
                {
                    selectedLevel2Category = value;
                    OnPropertyChanged();
                    UpdateLevel3Categories();
                    UpdateSelectedCategoryId();
                    IsLevel3Visible = selectedLevel2Category != null;
                }
            }
        }

        private Category selectedLevel3Category;
        public Category SelectedLevel3Category
        {
            get => selectedLevel3Category;
            set
            {
                if (selectedLevel3Category != value)
                {
                    selectedLevel3Category = value;
                    OnPropertyChanged();
                    UpdateLevel4Categories();
                    UpdateSelectedCategoryId();
                    IsLevel4Visible = selectedLevel3Category != null;
                }
            }
        }

        private Category selectedLevel4Category;
        public Category SelectedLevel4Category
        {
            get => selectedLevel4Category;
            set
            {
                if (selectedLevel4Category != value)
                {
                    selectedLevel4Category = value;
                    OnPropertyChanged();
                    UpdateSelectedCategoryId();
                }
            }
        }

        private string searchQuery;
        public string SearchQuery
        {
            get => searchQuery;
            set
            {
                if(searchQuery != value)
                {
                    searchQuery = value;
                    OnPropertyChanged();
                    FilterCategories();
                }
            }
        }

        #endregion

        #region Visibility Properties

        private bool isLevel2Visible;
        public bool IsLevel2Visible
        {
            get => isLevel2Visible;
            set
            {
                isLevel2Visible = value;
                OnPropertyChanged();
            }
        }

        private bool isLevel3Visible;
        public bool IsLevel3Visible
        {
            get => isLevel3Visible;
            set
            {
                isLevel3Visible = value;
                OnPropertyChanged();
            }
        }

        private bool isLevel4Visible;
        public bool IsLevel4Visible
        {
            get => isLevel4Visible;
            set
            {
                isLevel4Visible = value;
                OnPropertyChanged();
            }
        }
        #endregion

        public MainViewModel()
        {
            LoadCategories();
            IsLevel2Visible = false;
            IsLevel3Visible = false;
            IsLevel4Visible = false;
        }

        #region Load and Filter Category Methods

        // TODO: I would create a CategoryService and move these methods into it, more reusable KLH
        private void FilterCategories()
        {
            if (string.IsNullOrWhiteSpace(SearchQuery))
            {
                FilteredLevel1Categories = new ObservableCollection<Category>(Level1Categories);
            }
            else
            {
                FilteredLevel1Categories = new ObservableCollection<Category>(Level1Categories
                    .Where(c => c.Name.ToLower().Contains(SearchQuery.ToLower())));
            }
            OnPropertyChanged(nameof(FilteredLevel1Categories));
        }

        private void LoadCategories()
        {
            var rawCategoryList = CategoryList.LowerCategories;

            var categoryHierarchy = CategoryParser.BuildCategoryHierarchy(rawCategoryList, useAIName: false);

            foreach (var category in categoryHierarchy)
            {
                Level1Categories.Add(category);
            }

            // Initialize with all categories in the list
            FilteredLevel1Categories = new ObservableCollection<Category>(Level1Categories);
        }
        #endregion

        #region Update Category Methods

        private void UpdateLevel2Categories()
        {
            Level2Categories.Clear();
            FilteredLevel2Categories.Clear();

            if(SelectedLevel1Category != null)
            {
                foreach(var level2category in SelectedLevel1Category.Level2Categories)
                {
                    Level2Categories.Add(level2category);
                    FilteredLevel2Categories.Add(level2category);
                }
            }

            SelectedLevel2Category = null;
        }

        private void UpdateLevel3Categories()
        {
            Level3Categories.Clear();
            FilteredLevel3Categories.Clear();

            if (SelectedLevel2Category != null)
            {
                foreach (var level3category in SelectedLevel2Category.Level2Categories)
                {
                    Level3Categories.Add(level3category);
                    FilteredLevel3Categories.Add(level3category);
                }
            }
            SelectedLevel3Category = null;
        }

        private void UpdateLevel4Categories()
        {
            Level4Categories.Clear();
            FilteredLevel4Categories.Clear();
            if (SelectedLevel3Category != null)
            {
                foreach (var level4category in SelectedLevel3Category.Level2Categories)
                {
                    Level4Categories.Add(level4category);
                    FilteredLevel4Categories.Add(level4category);
                }
            }
            SelectedLevel4Category = null;
        }

        private void UpdateSelectedCategoryId()
        {
            // Set ID only if 4th-level category is selected,
            // uncomment below and remove condition in parser to set L1,L2,etc. KLH
            SelectedCategoryId = SelectedLevel4Category?.catID ?? 0;
                                 //?? SelectedLevel3Category?.catID
                                 //?? SelectedLevel2Category?.catID
                                 //?? SelectedLevel1Category?.catID
                                 //?? 0;
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

