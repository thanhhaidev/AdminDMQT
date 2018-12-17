$(document).ready(function(){    
	(function($) { 
		$('#header__icon').click(function(e){            
			e.preventDefault();            
			$('body').toggleClass('with--sidebar');        
		});        
		$('#site-cache').click(function(e){
			$('body').removeClass('with--sidebar');
		});    
	})
	(jQuery);});


$(document).ready(function() {
  var owl = $('.owl-carousel');
  owl.owlCarousel({
    loop: true,
    margin: 10,
    navRewind: false,
    responsive: {
      0: {
        items: 1
      },
      600: {
        items: 3
      },
      1000: {
        items: 5
      }
    }
  })
})

$('.bxslider').bxSlider({
  pagerCustom: '#bx-pager'
});
