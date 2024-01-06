
const rangeInput = document.querySelectorAll(".range-input input"),
  priceInput = document.querySelectorAll(".price-input input"),
  range = document.querySelector(".slider .progress");
let priceGap = 1000;

priceInput.forEach((input) => {
  input.addEventListener("input", (e) => {
    let minPrice = parseInt(priceInput[0].value),
      maxPrice = parseInt(priceInput[1].value);

    if (maxPrice - minPrice >= priceGap && maxPrice <= rangeInput[1].max) {
      if (e.target.className === "input-min") {
        rangeInput[0].value = minPrice;
        range.style.left = (minPrice / rangeInput[0].max) * 100 + "%";
      } else {
        rangeInput[1].value = maxPrice;
        range.style.right = 100 - (maxPrice / rangeInput[1].max) * 100 + "%";
      }
    }
  });
});

rangeInput.forEach((input) => {
  input.addEventListener("input", (e) => {
    let minVal = parseInt(rangeInput[0].value),
      maxVal = parseInt(rangeInput[1].value);

    if (maxVal - minVal < priceGap) {
      if (e.target.className === "range-min") {
        rangeInput[0].value = maxVal - priceGap;
      } else {
        rangeInput[1].value = minVal + priceGap;
      }
    } else {
      priceInput[0].value = minVal;
      priceInput[1].value = maxVal;
      range.style.left = (minVal / rangeInput[0].max) * 100 + "%";
      range.style.right = 100 - (maxVal / rangeInput[1].max) * 100 + "%";
    }
  });
});




function AutoClose() {
    $(".mobile-nav-bar").hide();
}

AutoClose();


$("#mobile-active-btn").click(function(e){
    e.preventDefault()
    $(".mobile-nav-bar").show("slide");
    console.log(e.target)
})


$(".close-mob-nav").click(function(e){
    console.log("s")
    e.preventDefault()
    $(".mobile-nav-bar").hide("slide");

})



$('.opre').on("click", function(e){

    e.preventDefault();
    // // modal.style.display = "block";
    // const container = document.querySelector('.container');
    // container.classList.toggle('active');

    $("#change-log").toggleClass("active")
  })

  $('.closre').on("click", function(e){

    e.preventDefault();
    // // modal.style.display = "block";
    // const container = document.querySelector('.container');
    // container.classList.toggle('active');

    $("#change-log").toggleClass("active")
  })

  