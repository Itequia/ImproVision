
$(document).ready(function () {
	initDropzone();
});


function initDropzone() {
	// Disabling autoDiscover, otherwise Dropzone will try to attach twice.
	Dropzone.autoDiscover = false;

	// Dropzonejs 
	$("#my-dropzone").dropzone({
		url: $("#my-dropzone").data("url"),
		maxFiles: 10,
		maxFilesize: 5, // MB
		autoProcessQueue: false,
		uploadMultiple: true,
		parallelUploads: 10,
		acceptedFiles: "image/*",
		addRemoveLinks: false,
		dictMaxFilesExceeded: "Maximum upload limit reached (max 10 files)",
		dictFileTooBig: "File too big",
		dictInvalidFileType: "Invalid file type",
		previewTemplate: document.getElementById('preview-template').innerHTML,
		renameFile: function (file) {
			var timeInMs = Date.now();
			return timeInMs + "_" + file.name;
		},
		init: function () {
			var myDropzone = this;

			myDropzone.on("error", function (file, errorMessage) {
				if ((errorMessage == myDropzone.options.dictInvalidFileType) ||
					(errorMessage == myDropzone.options.dictMaxFilesExceeded) ||
					(errorMessage == myDropzone.options.dictFileTooBig)) {
					this.removeFile(file);
				}
			});

            myDropzone.on("successmultiple", function (files, response) {
                console.log(response);
                $("#blackboard-container").html(response);
				// Gets triggered when the files have successfully been sent.
				//var redirect = $('#btn-upload').data('url');
				//window.location = redirect;
			});

			myDropzone.on("errormultiple", function (files, response) {
				// Gets triggered when there was an error sending the files.
				// Maybe show form again, and notify user of error
				Metronic.unblockUI('.page-content-wrapper');
			});
		},
		success: function (file, response) {
			file.previewElement.classList.add("dz-success");
		},
		error: function (file, response) {
			file.previewElement.classList.add("dz-error");
			$(file.previewElement).find('.dz-error-message').text(response);
		}
	});
};

function UploadFiles(data) {
	var myDropzone = Dropzone.forElement(".dropzone");
	var queuedFiles = myDropzone.getQueuedFiles();

	if (queuedFiles.length == 0) {
		var redirect = $('#btn-upload').data('url');
		window.location = redirect;
	}
	else if (queuedFiles.length > 0) {
		myDropzone.options.url;
		myDropzone.processQueue();
	}
};
