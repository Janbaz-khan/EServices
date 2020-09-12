
function Refresh(data,url) {
    setTimeout(function () {
        data.load(url);
    }, 1000)
}

function insertIn(link, formData) {
    $.ajax({
        type: 'POST',
        url: link,
        data: formData,
        success: function (response) {
            
        }
    })
}