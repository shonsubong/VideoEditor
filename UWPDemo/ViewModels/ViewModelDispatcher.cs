using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWPDemo.ViewModels
{
    /// <summary>
    ///
    /// </summary>
    static class ViewModelDispatcher
    {        
        private static Lazy<VideoLibraryViewModel> videoLibraryViewModel;
        private static Lazy<VideoPreviewViewModel> videoPreviewViewModel;
        private static Lazy<StoryBoardViewModel> storyBoardViewModel;
        private static Lazy<VideoManagerVM> videoManager;


        /// <summary>
        /// Ensures there is one Video Manager view model and its only created when referenced.
        /// </summary>
        public static VideoManagerVM VideoManager
        {
            get
            {
                if (videoManager == null)
                {
                    videoManager = new Lazy<VideoManagerVM>();
                }
                return videoManager.Value;
            }
        }

        /// <summary>
        /// Ensures there is one Video Library view model and its only created when referenced.
        /// </summary>
        public static VideoLibraryViewModel VideoLIbraryViewModel
        {
            get
            {
                if (videoLibraryViewModel == null)
                {
                    videoLibraryViewModel = new Lazy<VideoLibraryViewModel>();
                }
                return videoLibraryViewModel.Value;
            }
        }

        /// <summary>
        /// Ensures there is one Video Preview view model and its only created when referenced.
        /// </summary>
        public static VideoPreviewViewModel VideoPreviewViewModel
        {
            get
            {
                if (videoPreviewViewModel == null)
                {
                    videoPreviewViewModel = new Lazy<VideoPreviewViewModel>();
                }
                return videoPreviewViewModel.Value;
            }
        }

        /// <summary>
        /// Ensures there is one Story Board view model and its only created when referenced.
        /// </summary>
        public static StoryBoardViewModel StoryBoardViewModel
        {
            get
            {
                if (storyBoardViewModel == null)
                {
                    storyBoardViewModel = new Lazy<StoryBoardViewModel>();
                }
                return storyBoardViewModel.Value;
            }
        }
    }
}
