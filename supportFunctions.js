function setFilePath(url){
    if (url == '/'){
        return 'index.html';
    } else if (url == '/login'){
        return 'login.html'
    } else {
        return url.slice(1);
    }
}

export default {
    setFilePath
};