
window.scrollToElement = (el) => {
    el?.scrollIntoView({ behavior: "instant", block: "center", inline: "center" });
}

window.observeResize = (el, dotnetRef, callbackName) => {
    
    if (!el) return;   
    
    const observer = new ResizeObserver(() => {
       dotnetRef.invokeMethodAsync(callbackName); // maybe throttle in future
    });
    
    observer.observe(el, {
        box: "border-box",
    });
    
}