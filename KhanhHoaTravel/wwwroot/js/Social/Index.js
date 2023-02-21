function showPostForm() {
    document.getElementById('overlay').style.display = "block";
    document.getElementById('add-post-form').style.display = "block";
}

function showCmtForm(postId) {
    document.getElementById('overlay').style.display = "block";
    let postSelectId = 'post' + postId;
    document.getElementById(postSelectId).style.display = "block";
    event.preventDefault();
}

function hideForm() {
    document.getElementById('overlay').style.display = "none";
    var posts = document.getElementsByClassName("PostComment-container");
    for (var i = 0; i < posts.length; i++) {
        posts[i].style.display = "none";
    }
    document.getElementById('add-post-form').style.display = "none";
    
}

function addCmt(postId, userName, userImagePath) {
    event.preventDefault(); // Chặn lại sự kiện mặc định của form
    let commentContentId = '#inputCmt' + postId;
    const content = document.querySelector(commentContentId).value;

    // Tạo một phần tử mới chứa thông tin của bình luận
    let newComment = document.createElement("div");
    newComment.classList.add("row", "item-column");
    newComment.innerHTML = `
    <img class="cmt-avt" src="${userImagePath}" />
        <div class="collumn cmt-box">
            <p class="cmt-name">${userName}</p>
            <p class="cmt-content">${content}</p>
        </div>
    `;

    // Thêm phần tử mới vào danh sách bình luận
    let commentaddId = '#comment-show' + postId;
    let commentShow = document.querySelector(commentaddId)
    // let commentShow = event.target.querySelector(commentaddId);
    commentShow.appendChild(newComment);
    document.querySelector(commentContentId).value = '';
}


/*Load Image preview when add to post*/
function loadImg() {
    const file = document.getElementById("imageUpload").files[0];
    const reader = new FileReader();
    reader.addEventListener('load', function () {
        document.getElementById("imgPreview").src = reader.result;

    });
    reader.readAsDataURL(file);
}