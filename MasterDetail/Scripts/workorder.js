$(function () {
    $("a[data-modal]").on("click", function () {
        $("#partsModalContent").load(this.href, function () {
            $("#partsModal").modal({ keyboad: true }, "show");
            $("#partchoice").submit(function () {
                if ($("#partchoice").valid()) {
                    $.ajax({
                        url: this.action,
                        type: this.method,
                        data: $(this).serialize(),
                        success: function (result) {
                            if (result.success) {
                                $("#partsModal").modal("hide");
                                location.reload();
                            }
                        },
                        error: function () {
                            $("#MessageToClient").text("The web server had an error.");
                        }
                    });
                    return false;
                }
            });
        });
        return false;
    });
});