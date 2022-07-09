
function AddFriend(frid) {
    let urlfrom = window.location.href
    let url = window.location.origin
    let urlto = url + "/Users/AddFriend?urlFrom=" + urlfrom + "&frId=" + frid

    console.log(urlfrom)
    console.log(urlto)

    window.location.replace(urlto)
}

function AddComment(text, pid) {
    let urlfrom = window.location.href
    let url = window.location.origin
    let urlto = url + "/Posts/AddComment?urlFrom=" + urlfrom + "&text=" + text+"&pid="+pid

    console.log(urlfrom)
    console.log(urlto)

    window.location.replace(urlto)
}