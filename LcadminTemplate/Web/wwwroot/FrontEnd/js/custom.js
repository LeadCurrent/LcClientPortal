// Sticky header
$(window).scroll(function () {
  if ($(window).scrollTop() > 0) {
    $("header").addClass('sticky');
  } else {
    $("header").removeClass('sticky');
  }
});

// AOS for Animation
AOS.init({
  disable: function () {
     var maxWidth = 768;
     return window.innerWidth < maxWidth;
  }
});







