export function scrollIntoView(element) {
    if (element) {
        element.scrollIntoView({
            behavior: 'smooth',
            block: 'nearest',
            inline: 'start'
        });
    }
}