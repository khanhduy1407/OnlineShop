// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
  $('.ui.sidebar').sidebar('attach events', '.item.hamberger');

  $('.ui.dropdown').dropdown();

  $('.message .close')
    .on('click', function () {
      $(this)
        .closest('.message')
        .transition('fade')
        ;
    });
});
