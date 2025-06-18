function ThongBao(message) {
    let toast = document.getElementById("thongbao");
    if (!toast) {
        toast = document.createElement("div");
        toast.id = "thongbao";
        document.body.appendChild(toast);
    }

    toast.textContent = message;
    toast.style.top = "20px";

    setTimeout(() => {
        toast.style.top = "-100px";
    }, 2000);
}
