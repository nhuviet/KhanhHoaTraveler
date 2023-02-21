// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

const chatbox = document.querySelector('.chatbox');
const openBtn = document.querySelector('#open-btn');
const closeBtn = document.querySelector('#cb-close-btn');
const messageInput = document.querySelector('#message-input');
const sendBtn = document.querySelector('#send-btn');
const messages = document.querySelector('.messages');

function getResponse(mess) {
    let message = mess;
    // Gửi prompt đến API Playground của OpenAI để lấy phản hồi
    //https://platform.openai.com/playground/p/s73Wb5ZkuZstXFHZhLfqw3Md?model=text-davinci-003
    fetch('https://api.openai.com/v1/playground/p/s73Wb5ZkuZstXFHZhLfqw3Md/response/', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
            data: {
                model: 'text-davinci-003',
                prompt: message,
                temperature: 0.7,
                max_tokens: 60
            }
        })
    })
        .then(response => response.json())
        .then(data => {
            const chatbotReply = data.choices[0].text.trim();
            replyEl.textContent = chatbotReply;
        });
    return message;
}

sendBtn.addEventListener('click', () => {
    const message = messageInput.value;
    if (message) {
        const messageEl = document.createElement('p');
        messageEl.textContent = 'Bạn: ';
        messageEl.textContent += message;
        messages.appendChild(messageEl);
        messageInput.value = '';

        const replyEl = document.createElement('p');
        replyEl.textContent = 'Bot: ';
        replyEl.textContent += getResponse(message);
        messages.appendChild(replyEl);

    }
});

function showchatbox() {
    document.getElementById('KHTChatbox').style.display = 'block';
}
function hidechatbox() {
    document.getElementById('KHTChatbox').style.display = 'none';
}