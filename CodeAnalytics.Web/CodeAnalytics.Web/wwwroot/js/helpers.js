
window.scrollToElement = (el) => {
    el?.scrollIntoView({ behavior: "instant", block: "center", inline: "center" });
}
