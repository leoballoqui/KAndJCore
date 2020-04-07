$(document).ready(function () {

        $('#drag-and-drop-zone').dmUploader({ //
            url: '/documents/Upload',
            maxFileSize: 3000000, // 3 Megs max
            multiple: false,
            //allowedTypes: 'image/*',
            extFilter: ['jpg', 'jpeg', 'png', 'gif', 'pdf'],
            onDragEnter: function () {
                // Happens when dragging something over the DnD area
                this.addClass('active');
            },
            onDragLeave: function () {
                // Happens when dragging something OUT of the DnD area
                this.removeClass('active');
            },
            onInit: function () {
                // Plugin is ready to use
                this.find('input[type="text"]').val('');
            },
            onComplete: function () {
                // All files in the queue are processed (success or error)
            },
            onNewFile: function (id, file) {
                // When a new file is added using the file selector or the DnD area
                if (typeof FileReader !== "undefined") {
                    var reader = new FileReader();
                    var img = this.find('img');
                    reader.onload = function (e) {
                        if (file.type === "application/pdf")
                            img.attr('src', "../../documents/thumbnails/pdf.png");
                        else
                            img.attr('src', e.target.result);
                    };
                    reader.readAsDataURL(file);
                }
            },
            onBeforeUpload: function (id) {
                // about tho start uploading a file
                ui_single_update_progress(this, 0, true);
                ui_single_update_active(this, true);
                ui_single_update_status(this, 'Uploading...');
            },
            onUploadProgress: function (id, percent) {
                // Updating file progress
                ui_single_update_progress(this, percent);
            },
            onUploadSuccess: function (id, data) {
                var response = JSON.stringify(data);
                // A file was successfully uploaded
                ui_single_update_active(this, true);
                ui_single_update_reset(this);
                
                this.find('input[type="text"]').val(data.fileName);
                ui_single_update_status(this, 'Upload completed.', 'success');

                $("#FileFullName").val(data.fileName);
            },
            onUploadError: function (id, xhr, status, message) {
                // Happens when an upload error happens
                ui_single_update_active(this, false);
                ui_single_update_status(this, 'Error: ' + message, 'danger');
            },
            onFallbackMode: function () {
                // When the browser doesn't support this plugin :(
            },
            onFileSizeError: function (file) {
                ui_single_update_status(this, 'File excess the size limit', 'danger');
            },
            onFileTypeError: function (file) {
                ui_single_update_status(this, 'File type is not an image', 'danger');
            },
            onFileExtError: function (file) {
                ui_single_update_status(this, 'File extension not allowed', 'danger');
            }
        });

        function ui_single_update_active(element, active) {
            element.find('div.progress').toggleClass('d-none', !active);
            element.find('input[type="text"]').toggleClass('d-none', active);

            element.find('input[type="file"]').prop('disabled', active);
            element.find('.btn').toggleClass('disabled', active);

            element.find('.btn i').toggleClass('fa-circle-o-notch fa-spin', active);
            element.find('.btn i').toggleClass('fa-folder-o', !active);
        }   

        function ui_single_update_reset(element) {
            element.find('input[type="file"]').prop('disabled', false);
            element.find('.btn').toggleClass('disabled', false);
            element.find('.btn i').toggleClass('fa-circle-o-notch fa-spin', false);
            element.find('.btn i').toggleClass('fa-folder-o', true);
        }

        function ui_single_update_progress(element, percent, active) {
            active = (typeof active === 'undefined' ? true : active);

            var bar = element.find('div.progress-bar');

            bar.width(percent + '%').attr('aria-valuenow', percent);
            bar.toggleClass('progress-bar-striped progress-bar-animated', active);

            if (percent === 0) {
                bar.html('');
            } else {
                bar.html(percent + '%');
            }
        }

        function ui_single_update_status(element, message, color) {
            color = (typeof color === 'undefined' ? 'muted' : color);

            element.find('small.status').prop('class', 'status text-' + color).html(message);
        }
});