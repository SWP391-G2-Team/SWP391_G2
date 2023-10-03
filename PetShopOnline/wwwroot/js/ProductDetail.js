//// Lấy các phần tử DOM cần sử dụng
//const mainImage = document.getElementById("#product-img img");
//const imageItems = document.querySelectorAll("#product-img-items img");

//// Xử lý sự kiện nhấp chuột vào hình ảnh nhỏ
//imageItems.forEach((imageItem) => {
//    imageItem.addEventListener("click", function () {
//        const imageUrl = this.getAttribute("src");
//        mainImage.setAttribute("src", imageUrl);
//    });
//});
function changeMainImage(element) {
    var mainImage = document.getElementById('mainImage');
    mainImage.src = element.src;
}