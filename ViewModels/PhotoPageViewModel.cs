using Avalonia;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Avalonia.Controls.ApplicationLifetimes;
using MunicipalApp.Services;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace MunicipalApp.ViewModels
{
    public class PhotoPageViewModel : ViewModelBase
    {
        private readonly List<PhotoFile> _uploadedFiles;
        private bool _showUploadSuccess;
        private ReactiveCommand<Unit, Unit> _uploadPhotosCommand;
        private ReactiveCommand<Unit, Unit> _clearAllCommand;
        private ReactiveCommand<PhotoFile, Unit> _removePhotoCommand;

        public PhotoPageViewModel()
        {
            _uploadedFiles = new List<PhotoFile>();
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            _uploadPhotosCommand = ReactiveCommand.CreateFromTask(UploadPhotos);
            _clearAllCommand = ReactiveCommand.Create(ClearAllPhotos);
            _removePhotoCommand = ReactiveCommand.Create<PhotoFile, Unit>(RemovePhoto);
        }

        public List<PhotoFile> UploadedFiles => _uploadedFiles;
        public bool HasPhotos => _uploadedFiles.Count > 0;
        public string FileCountText => $"{_uploadedFiles.Count}/10 photos uploaded";
        public bool ShowUploadSuccess => _showUploadSuccess;

        private async Task UploadPhotos()
        {
            var topLevel = TopLevel.GetTopLevel((Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow ?? throw new InvalidOperationException());
            if (topLevel == null) return;

            var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = "Select Photos",
                FileTypeFilter = new FilePickerFileType[]
                {
                    new FilePickerFileType("Images")
                    {
                        Patterns = new[] { "*.jpg", "*.jpeg", "*.png", "*.gif" },
                        MimeTypes = new[] { "image/jpeg", "image/png", "image/gif" }
                    }
                },
                AllowMultiple = true
            });

            if (files.Count == 0) return;

            var addedCount = 0;
            foreach (var file in files)
            {
                if (_uploadedFiles.Count >= 10) break;

                try
                {
                    var fileInfo = new FileInfo(file.Path.LocalPath);
                    
                    // Check file size (5MB limit)
                    if (fileInfo.Length > 5 * 1024 * 1024)
                    {
                        continue; // Skip files larger than 5MB
                    }

                    // Copy file to app directory
                    var fileName = $"{Guid.NewGuid()}_{file.Name}";
                    var destinationPath = Path.Combine("Data", "Photos", fileName);
                    
                    // Ensure directory exists
                    var directory = Path.GetDirectoryName(destinationPath);
                    if (!string.IsNullOrEmpty(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }
                    
                    // Copy file
                    File.Copy(file.Path.LocalPath, destinationPath, true);

                    var photoFile = new PhotoFile
                    {
                        FileName = file.Name,
                        FilePath = destinationPath,
                        FileSize = fileInfo.Length,
                        Preview = destinationPath // In a real app, you'd generate a thumbnail
                    };

                    _uploadedFiles.Add(photoFile);
                    addedCount++;
                }
                catch (Exception)
                {
                    // Handle file copy errors
                }
            }

            if (addedCount > 0)
            {
                _showUploadSuccess = true;
                // Hide success message after 3 seconds
                Task.Delay(3000).ContinueWith(_ => 
                {
                    _showUploadSuccess = false;
                    this.RaisePropertyChanged(nameof(ShowUploadSuccess));
                });
            }

            this.RaisePropertyChanged(nameof(UploadedFiles));
            this.RaisePropertyChanged(nameof(HasPhotos));
            this.RaisePropertyChanged(nameof(FileCountText));
            this.RaisePropertyChanged(nameof(ShowUploadSuccess));
        }

        private Unit RemovePhoto(PhotoFile photoFile)
        {
            _uploadedFiles.Remove(photoFile);
            
            // Delete the file from disk
            try
            {
                if (File.Exists(photoFile.FilePath))
                {
                    File.Delete(photoFile.FilePath);
                }
            }
            catch (Exception)
            {
                // Handle file deletion errors
            }

            this.RaisePropertyChanged(nameof(UploadedFiles));
            this.RaisePropertyChanged(nameof(HasPhotos));
            this.RaisePropertyChanged(nameof(FileCountText));
            
            return Unit.Default;
        }

        private void ClearAllPhotos()
        {
            // Delete all files from disk
            foreach (var photoFile in _uploadedFiles)
            {
                try
                {
                    if (File.Exists(photoFile.FilePath))
                    {
                        File.Delete(photoFile.FilePath);
                    }
                }
                catch (Exception)
                {
                    // Handle file deletion errors
                }
            }

            _uploadedFiles.Clear();
            this.RaisePropertyChanged(nameof(UploadedFiles));
            this.RaisePropertyChanged(nameof(HasPhotos));
            this.RaisePropertyChanged(nameof(FileCountText));
        }

        public void Clear()
        {
            ClearAllPhotos();
            _showUploadSuccess = false;
            this.RaisePropertyChanged(nameof(ShowUploadSuccess));
        }

        public ReactiveCommand<Unit, Unit> UploadPhotosCommand => _uploadPhotosCommand;
        public ReactiveCommand<Unit, Unit> ClearAllCommand => _clearAllCommand;
        public ReactiveCommand<PhotoFile, Unit> RemovePhotoCommand => _removePhotoCommand;
    }

    public class PhotoFile
    {
        public string FileName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public long FileSize { get; set; }
        public string Preview { get; set; } = string.Empty;
    }
}