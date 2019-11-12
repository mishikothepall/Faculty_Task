var slideIndex = 0;

function Message() {
    alert("Заявка отправленна на рассмотрение администрации");
}


function readURL(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            document.getElementById('ph').value = e.target.result;
            $('#studPhoto').attr('src', e.target.result);
        };

        reader.readAsDataURL(input.files[0]);
    }
}

$("#imgInp").change(function () {
    readURL(this);
});
document.onload = showSlides(slideIndex);


window.addEventListener('scroll', function () {
    let len = pageYOffset;
    if (len >= 10) {
        document.getElementById('myBtn').style.visibility = "visible";
    }
    else {
        document.getElementById('myBtn').style.visibility = "hidden";
    }
});




function plusSlides(n) {
    showSlides(slideIndex += n);
}

function showSlides(n) {
    let i;
    let slides = document.getElementsByClassName("slide");

    if (n > slides.length) { slideIndex = 1; }
    if (n < 1) { slideIndex = slides.length; }
    for (i = 0; i < slides.length; i++) {
        slides[i].style.display = "none";
    }
    slides[slideIndex - 1].style.display = "block";

    
}





function topFunction() {
    document.body.scrollTop = 0;
    document.documentElement.scrollTop = 0;
    document.getElementById("#myBtn").style.visibility = "hidden";
}




  