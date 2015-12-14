$(function () {
    $("a[data-modal]").on("click", function () {
        $("#partsModalContent").load(this.href, function () {
            $("#partsModal").modal({ keyboad: true }, "show");
        });
        return false;
    });
});