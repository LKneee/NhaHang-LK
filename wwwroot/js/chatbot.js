document.addEventListener("DOMContentLoaded", function () {
    const btn = document.getElementById("chat-button");
    const panel = document.getElementById("chat-panel");
    const closeBtn = document.getElementById("close-chat");
    const sendBtn = document.getElementById("chat-send");
    const input = document.getElementById("chat-text");
    const messages = document.getElementById("chat-messages");

    const appendMessage = (who, text) => {
        const line = document.createElement("div");
        line.className = "chat-line " + who;

        if (who === "bot") {
            const avatar = document.createElement("img");
            avatar.src = "/image/logo.jpg";
            avatar.className = "chat-avatar";
            line.appendChild(avatar);
        }

        const msg = document.createElement("div");
        msg.className = "chat-msg " + who;
        msg.innerHTML = text; 
        line.appendChild(msg);

        messages.appendChild(line);
        messages.scrollTop = messages.scrollHeight;

        localStorage.setItem("chatHistory", messages.innerHTML);
    };

    const savedChat = localStorage.getItem("chatHistory");
    if (savedChat) {
        messages.innerHTML = savedChat;
    } else {
        const welcome = "Xin chào <br> Mình là <b> trợ lý thông minh của Nhà Hàng LK</b>.<br>Rất vui khi được giúp đỡ bạn!<br>Bạn cần tôi hỗ trợ gì?";
        appendMessage("bot", welcome);
    }

    btn.onclick = () => {
        panel.style.display = panel.style.display === "none" ? "block" : "none";
    };
    closeBtn.onclick = () => (panel.style.display = "none");

    const send = () => {
        const msg = input.value.trim();
        if (!msg) return;
        appendMessage("user", msg);
        input.value = "";

        fetch("/Chat/Ask", {
            method: "POST",
            headers: { "Content-Type": "application/x-www-form-urlencoded" },
            body: "message=" + encodeURIComponent(msg)
        })
            .then((r) => r.json())
            .then((data) => {
                if (data.items) {
                    appendMessage("bot", "Các món bạn hỏi gồm:");
                    data.items.forEach((x) => {
                        const price = x.gia ? `${x.gia.toLocaleString()} VNĐ` : "";
                        appendMessage("bot", `${x.tenMon} (${price})`);
                    });
                } else {
                    appendMessage("bot", data.reply || "Không tìm thấy món phù hợp.");
                }
            })
            .catch((err) => appendMessage("bot", "Lỗi: " + err.message));
    };

    sendBtn.onclick = send;
    input.addEventListener("keypress", (e) => e.key === "Enter" && send());

    const menuIcon = document.getElementById("menu-icon");
    const chatMenu = document.getElementById("chat-menu");
    const reloadBtn = document.getElementById("reload-chat");

    menuIcon.onclick = () => chatMenu.classList.toggle("hidden");

    reloadBtn.onclick = () => {
        localStorage.removeItem("chatHistory");
        const messages = document.getElementById("chat-messages");
        messages.innerHTML = "";

        const welcome = "Xin chào <br> Mình là trợ lý thông minh của Nhà Hàng LK. <br> Rất vui khi được giúp đỡ bạn.Bạn cần tôi hỗ trợ gì?";
        const line = document.createElement("div");
        line.className = "chat-line bot";

        const avatar = document.createElement("img");
        avatar.src = "/image/logo.jpg";
        avatar.className = "chat-avatar";
        line.appendChild(avatar);

        const msg = document.createElement("div");
        msg.className = "chat-msg bot";
        msg.innerHTML = welcome;
        line.appendChild(msg);

        messages.appendChild(line);
        messages.scrollTop = messages.scrollHeight;

        localStorage.setItem("chatHistory", messages.innerHTML);

        chatMenu.classList.add("hidden");
    };

});
