
function showEditForm() {
    document.getElementById("overlay").style.display = 'block';
    document.getElementById("editForm").style.display = 'block';
}

function hideEditForm() {
    document.getElementById("overlay").style.display = 'none';
    document.getElementById("editForm").style.display = 'none';
}

function loadImg() {
    const file = document.getElementById("imageUpload").files[0];
    const reader = new FileReader();
    reader.addEventListener('load', function () {
        document.getElementById("imagePreview").style.backgroundImage = "url('" + reader.result + "'";

    });
    reader.readAsDataURL(file);
}
