/**
 * Created by Niklas Grieger on 02.12.2016.
 * js for the Chat Widget
 */
var Chat = Chat || (function () {
    var allCategoriesDataSource;
    var allUserDatasource;
    var empfaenger_name;
    var currentUserDataSource = null;
    var adminId = 0;
    var empfaenger_id;
    // Reference the auto-generated proxy for the hub.  
    var chat = $.connection.chatHub;
    var notification = $.connection.notificationHub;
    function fillUserlist() {
        /// <summary>
        /// Fills the friendlist.
        /// </summary>
        var categorylist = [];
        $.ajax({
            url: "/Chatlogs/GetAllSupportUser",
            success: function (data) {
                $.each(data,
                    function (i, item) {
                        //Verhindert, dass der CurrentUser in der Liste erscheint
                        if (item.user_id != currentUserDataSource.user_id) {
                            var categoryItem = "<div id='" + item.user_id + "' class='friend'>" +
                                                    //"<img src='" + item.avatarlink + "' alt='Kein Bild!'/>" +
                                                        "<p>" +
                                                            "<strong>" + item.category + "</strong></br>" +
                                                        "</p>" +
                                                        "<span id='user_name'>"+item.user_name+"</span>" +
                                                        "<div class='status available'></div>" +
                                                    "</div>";
                            categorylist.push(categoryItem);
                        }
                    });
                allSupportUserDataSource = data;
                console.log("allSupportUserDataSource:");
                console.log(allSupportUserDataSource);
                var searchbox = "<div id='search'>" +
                                    "<input type='text' id='searchfield' placeholder='Search contacts...' />" +
                                "</div>";
                categorylist.push(searchbox);
                $("#friends").html(categorylist);
            },
            cache: false
        });
    }
    function fillSupportList() {
        /// <summary>
        /// Fills the friendlist.
        /// </summary>
        var categorylist = [];
        $.ajax({
            //?empfaenger_id=" + localStorage.getItem("currentUserId")
            url: "/Users/GetUserWithoutSupport?currentUserId=" + currentUserDataSource.user_id,
            success: function (data) {
                $.each(data,
                    function (i, item) {
                        //Verhindert, dass der CurrentUser in der Liste erscheint
                        if (item.user_id != currentUserDataSource.user_id) {
                            var categoryItem = "<div id='" + item.user_id + "' class='friend'>" +
                                                    "<img src='" + item.avatarlink + "' alt='Kein Bild!'/>" +
                                                        "<span id='user_name'>" + item.first_name + " " + item.last_name + "</span>" +
                                                        "<span>"+item.NewMessages+"</span>"+
                                                        "<div class='status available'></div>" +
                                                    "</div>";
                            categorylist.push(categoryItem);
                        }
                    });
                allSupportUserDataSource = data;
                console.log("allSupportUserDataSource:");
                console.log(allSupportUserDataSource);
                var searchbox = "<div id='search'>" +
                                    "<input type='text' id='searchfield' placeholder='Search contacts...' />" +
                                "</div>";
                categorylist.push(searchbox);
                $("#friends").html(categorylist);
            },
            cache: false
        });
    }
    function loadMessagesUser2User() {
        /// <summary>
        /// Loads the messages user2 user.
        /// </summary>
        var timenow = kendo.toString(new Date, "dd.MM.yyyy HH:mm:ss");
        var chatlogu2u = [];
        var sender_id = currentUserDataSource.user_id;
        var dataString = 'sender_id=' + sender_id + '&empfaenger_id=' + empfaenger_id;
        $.ajax({
            type: 'GET',
            data: dataString,
            url: "/Chatlogs/GetChatUser2User",
            success: function (data) {
                console.log("GetChatUser2User: ");
                console.log(data);
                var chatlogu2uItem = "";
                var today = kendo.toString(new Date(), "dd. MMMM");
                var todayDay = kendo.toString(new Date(), "d");
                chatlogu2uItem = "<label>" + today + "</label>";
                chatlogu2u.push(chatlogu2uItem);
                $.each(data,
                    function (i, item) {
                            var chatlogu2uItem = "";
                            var timeFromNow = kendo.toString(kendo.parseDate(item.timestamp, "H:mm"), "H:mm");
                            console.log(item);
                            console.log(currentUserDataSource.user_id);
                            // == da der localstorage im Format String ist und nicht den gleichen Typ hat
                            if (currentUserDataSource.user_id == item.empfaenger_id) {
                                console.log("empfenger: " + item.empfaenger_id);
                                chatlogu2uItem = "<div class='message'>" +
                                                    "<img src='' />" +
                                                    "<div class='bubble'> " +
                                                        item.message +
                                                        "<div class='corner'></div> " +
                                                        "<span>" + timeFromNow + "</span>" +
                                                    "</div>" +
                                                 "</div>";
                            }
                            else if (item.sender_id == currentUserDataSource.user_id) {
                                console.log("sender: " + item.sender_id);
                                chatlogu2uItem = "<div class='message right'>" +
                                                    "<div class='bubble'>" +
                                                        item.message +
                                                        "<div class='corner'></div>" +
                                                        "<span>" + timeFromNow + "</span>" +
                                                    "</div>" +
                                                 "</div>";
                            }
                            chatlogu2u.push(chatlogu2uItem);
                        
                    });
                console.log("ChatUser2User:" + chatlogu2u);
                $("#chat-messages").html(chatlogu2u);
                console.log($("#chat-messages"));
            }
        });
        //Ab hier wird der Hub getriggert wenn eine Message ankommt
        //Scrollt nach unten beim ersten mal laden des Chats
        setTimeout(function () {
            $("#chat-messages").scrollTop($("#chat-messages").prop("scrollHeight"));
        }, 100);
        //if (localStorage.getItem("currentUserId") == sender_id || localStorage.getItem("currentUserId") != empfaenger_id){
            chat.client.addNewMessageToPage = function (sender_id, empfaenger_id, message, timestamp) {
                console.log("SignalR: addNewMessageToPage getriggert...");
                var chatlogu2uItem = "";
                // Add the message to the page. 
                //Wenn der der empfänger ist werden die Nachrichten links angezeigt
                if (currentUserDataSource.user_id == sender_id) {
                    console.log("local = " + sender_id);
                    chatlogu2uItem = "<div class='message right'>" +
                                            "<div class='bubble'>" +
                                                message +
                                                "<div class='corner'></div>" +
                                                "<span>" + timestamp + "</span>" +
                                            "</div>" +
                                            "</div>";
                    $('#chat-messages').append(chatlogu2uItem);
                }
                else if (currentUserDataSource.user_id == empfaenger_id) {
                    console.log("local= " + adminId);
                    chatlogu2uItem = "<div class='message'>" +
                                                    "<img src='' />" +
                                                    "<div class='bubble'> " +
                                                        message +
                                                        "<div class='corner'></div> " +
                                                        "<span>" + timestamp + "</span>" +
                                                    "</div>" +
                                                 "</div>";
                    $('#chat-messages').append(chatlogu2uItem);
                }
                console.log(chatlogu2uItem);
                $("#chat-messages").scrollTop($("#chat-messages").prop("scrollHeight"));
            }

        //Zum Real time Anzeigen [User] is Typing....
            chat.client.sayWhoIsTyping = function (name) {
                console.log("UserisTypingToClient" + name);
                $('#isTyping').html(name +" is typing...");
                setTimeout(function () {
                    $('#isTyping').html('&nbsp;');
                }, 5000);
            };
            $.connection.hub.start().done(function () {
                $('#message').keypress(function (e) {
                    if (e.which == 13) {
                        $('#sendmessage').trigger('click');
                    } else {
                        var encodedName = currentUserDataSource.full_name;
                        console.log("Typing User..."+encodedName);
                        chat.server.isTyping(encodedName);
                    }
                });
            });
        //}
    }
    function sendMessage2Support() {
        console.log("IM sendMessage2Support()");
        /// <summary>
        /// Sends the message user2 user.
        /// </summary>
        var timeNow = kendo.toString(new Date(), "H:mm");
        var sender_id = currentUserDataSource.user_id;
        var sender_name = currentUserDataSource.full_name
        if ($("#message").val() !== "") {
            //Start Hub to Send something
            $.connection.hub.start().done(function () {
                // Call the Send method on the hub. 
                chat.server.send(sender_id, empfaenger_id, $('#message').val(), timeNow);
                    console.log("Sende Notification an Server...")
                    notification.server.sendNotification("Neue Message von: " + sender_name, empfaenger_id);
                
                console.log("SignalR: ChatMessageSend getriggert...");
                //$('#send').click(function () {
                
                // Clear text box and reset focus for next comment. 
            var message = $("#message").val();
            var dataString = 'sender_id=' + sender_id + '&empfaenger_id=' + empfaenger_id + '&message=' + message;
                $.ajax({
                    type: 'POST',
                    data: dataString,
                    url: '/Chatlogs/SendMessage2Support',
                    success: function (data) {
                        var array = [sender_id, empfaenger_id, $('#message').val(), timeNow];
                        console.log("Array wurde zum Hub gesendet: " + array);
                        console.log("Folgende Message wurde an empfaenger_id: " + empfaenger_id + " gesendet: ");
                        console.log(message);
                        $('#message').val('').focus();
                    }
                });
            });
        }

        
    }
    function toggleChat() {
        /// <summary>
        /// Setzt den Focus auf das Suchfeld oder die MessageBox 
        /// Animiert beim Klick auf ein Kontakt den Übergang zum Privaten Chat
        /// </summary>
        var preloadbg = document.createElement("img");
        preloadbg.src = "https://s3-us-west-2.amazonaws.com/s.cdpn.io/245657/timeline1.png";

        $("#searchfield")
            .focus(function () {
                if ($(this).val() == "Search contacts...") {
                    $(this).val("");
                }
            });
        $("#searchfield")
            .focusout(function () {
                if ($(this).val() == "") {
                    $(this).val("Search contacts...");

                }
            });

        $("#sendmessage input")
            .focus(function () {
                if ($(this).val() == "Send message...") {
                    $(this).val("");
                }
            });
        $("#sendmessage input")
            .focusout(function () {
                if ($(this).val() == "") {
                    $(this).val("Send message...");

                }
            });

        setTimeout(function () {
            $(".friend")
                .each(function () {
                    $(this)
                        .click(function () {
                            var childOffset = $(this).offset();
                            var parentOffset = $(this).parent().parent().offset();
                            var childTop = childOffset.top - parentOffset.top;
                            var clone = $(this).find('img').eq(0).clone();
                            var top = childTop + 12 + "px";
                            
                            $(clone).css({ 'top': top }).addClass("floatingImg").appendTo("#chatbox");

                            setTimeout(function () {
                                $("#profile p").addClass("animate");
                                $("#profile").addClass("animate");
                            },
                                100);
                            setTimeout(function () {
                                $("#chat-messages").addClass("animate");
                                $('.cx, .cy').addClass('s1');
                                setTimeout(function () { $('.cx, .cy').addClass('s2'); }, 100);
                                setTimeout(function () { $('.cx, .cy').addClass('s3'); }, 200);
                            },
                                150);

                            $('.floatingImg')
                                .animate({
                                    'width': "68px",
                                    'left': '220px',
                                    'top': '180px'
                                },
                                    200);

                            var bezeichnung = $(this).find("p strong").html();
                            
                            
                            empfaenger_name = $(this).find("span#user_name").text();
                            empfaenger_id = $(this).attr('id');
                            var zuständiger = "<b>Sie spechen mit: " + empfaenger_name + "</b>";
                            console.log("empfaenger_name");
                            console.log(empfaenger_name);
                            adminId = $(this).attr('id');
                            //var email = $(this).find("p span").html();
                            $("#profile p").html(bezeichnung);
                            $("#profile span").html(zuständiger);
                            //$("#profile span").html(email);

                            $(".message").not(".right").find("img").attr("src", $(clone).attr("src"));
                            $('#friendslist').fadeOut();
                            $('#chatview').fadeIn();
                            setTimeout(function(){
                                //Scrollt zur letzten Nachricht
                                $("#chat-messages").scrollTop($("#chat-messages").prop("scrollHeight"));
                            },10);
                            
                            $('#close')
                                .unbind("click")
                                .click(function () {
                                    $("#chat-messages, #profile, #profile p").removeClass("animate");
                                    $('.cx, .cy').removeClass("s1 s2 s3");
                                    $('.floatingImg')
                                        .animate({
                                            'width': "40px",
                                            'top': top,
                                            'left': '12px'
                                        },
                                            200,
                                            function () { $('.floatingImg').remove() });

                                    setTimeout(function () {
                                        $('#chatview').fadeOut();
                                        $('#friendslist').fadeIn();
                                    },
                                        50);
                                });
                            loadMessagesUser2User();
                        });

                });
        }, 600);
    }
    function checkUserIsAdmin(currentUserId) {
        $.ajax({
            url: "/Users/CheckUserIsAdmin?user_id="+currentUserId,
            success: function (data) {
                console.log(data);
                if (data === "False") {
                    fillUserlist();
                }
                else if (data === "True") {
                    fillSupportList();
                }
            }
        })
    }
    function openChatUser2User(senderId, empfaengerId) {
        console.log("Öffne Chat...");
        console.log($(".friend#" + senderId).find("div"));
        $(".friend#" + senderId).find("div").trigger("click");
    }
    //function toggle() {
    //    var ele = $(".showMessage_fullMessage");
    //    var text = $("#showMessage_toggle");
    //    if (ele.css("display") === "block") {
    //        ele.css("display", "none");
    //        text.html("+");
    //    }
    //    else {
    //        ele.css("display", "block");
    //        text.html("-");
    //    }
    //}
    function getMessagesSinceLastLogin() {
        $.ajax({
            type: "GET",
            url: "/Chatlogs/GetMessagesSinceLastLogin?currentUserId=" + currentUserDataSource.user_id,
            success: function (data) {
                console.log("Initialisiere neue Notifications seit letztem Login...")
                
                var count = 0;
                if (data.length > 0) {
                    $.each(data, function (index, value) {
                        var message = "Neue Message von: " + allUserDatasource[value.sender_id - 1].user_name;
                        console.log(index + ". Message seit letztem Login: " + message)
                        $('#notiContent').append($("<li>"+
                                                        "<a style='text-decoration:none !important;' href='javascript:Chat.OpenChatUser2User(" + value.sender_id + ", " + value.empfaenger_id + ");'>" +
                                                            "Neue Message von: " + allUserDatasource[value.sender_id - 1].user_name+
                                                            "Message: " + kendo.toString(kendo.parseDate(value.timestamp, "H:mm"), "H:mm") + "-> " + value.message +
                                                        //<a id=\"showMessage_toggle\" class=\"k-button\" onclick=\"javascript:Chat.Toggle()\">+</a>" +
                                                        //    "<div class=\"showMessage_fullMessage"+index+"\" style=\"display: none\">" + kendo.toString(kendo.parseDate(value.timestamp, "H:mm"), "H:mm") + "-> " + value.message + "</div>" +
                                                        //"</div>"+
                                                        "</a>"+
                                                   "</li>"));
                        count++;
                    });
                }
                else {
                    console.log(data);
                    $('#notiContent').html($('<li>Keine Neuigkeiten</li>'));
                }
                $('span.count').html(count);
                
            }
        })
    }
    function saveLoginTime() {
        $.ajax({
            type: 'POST',
            url: "/Chatlogs/SaveLoginTime?currentUserId=" + currentUserDataSource.user_id,
            success: function () {
                console.log("Der Timestamp vom Userlogin ID=" + currentUserDataSource.user_id + " wurde geändert");
            },
            error: function () {
                console.log("Userlogin ID=" + currentUserDataSource.user_id + " Timestamp wurde nicht gesetzt");
            }
        });
    }
    function updateNotificationCount() {
        var count = 0;
        count = parseInt($('span.count').html()) || 0;
        count++;
        $('span.count').html(count);
    }
    function login() {
        currentUserId = $("#aktUser").data("kendoDropDownList").value();
        currentUserName = $("#aktUser").data("kendoDropDownList").text();
        console.log("currentUserName");
        console.log(currentUserName);
        localStorage.setItem("currentUserId", currentUserId);
        localStorage.setItem("currentUserName", currentUserName);
        checkUserIsAdmin(currentUserId);
        getMessagesSinceLastLogin();
        saveLoginTime();
        init();
    }
    function logout() {
        $('#close').trigger("click");
        localStorage.setItem("currentUserId", "");
        $("#top h4").html("");
        $("#friends").html("");
        $("#login").show();
        $("#chatContent").hide();
    }

    function init(currentUserIdParam) {
        alert(currentUserIdParam);
    if(currentUserIdParam != null){
        alert("currentUserIdParam: "+currentUserIdParam);
        $.ajax({
            type: "GET",
            url: "/Chatlogs/GetUserInfoById?currentUserId=" + currentUserIdParam,
            success: function (data) {
                currentUserDataSource = data;
                console.log(currentUserDataSource);
            },
            error: function (e) {
                console.log("Userinfos konnten für den User mit der ID: " + currentUserId + " nicht geladen werden.\n error: " + e);
            }
        });
        setTimeout(function () {
            checkUserIsAdmin(currentUserDataSource.user_id);
            alert("Hallo");
            $("#top h4").html("Sie melden sich als: <b>" + currentUserDataSource.full_name + "</b>");
            toggleChat();
        },100);
    }
    else{

    }
        //$("#aktUser").kendoDropDownList({
        //    dataSource: {
        //        transport: {
        //            read: function (e) {
        //                $.ajax({
        //                    url: "/Users/GetAllUser",
        //                    success: function (data) {
        //                        console.log(data);
        //                        allUserDatasource = data;
        //                        e.success(data);
        //                    }
        //                })
        //            }
        //        }
        //    },
        //    optionLabel: "Wählen Sie ihre Identität...",
        //    dataTextField: "user_name",
        //    dataValueField: "user_id"
        //});
        //console.log(localStorage);
        //if (localStorage.getItem("currentUserId") === null || localStorage.getItem("currentUserId") === "") {
        //    $("#top h4").html("");
        //    $("#friends").html("");
        //    $("#login").show();
        //    $("#chatContent").hide();
        //}
        //else {
        //    $("#login").hide();
        //    $("#chatContent").show();
        //    toggleChat();
        //    $("#top h4").html("Sie melden sich als: <b>" + localStorage.getItem("currentUserName") + "</b>");
        //}
    }
    return {
        Init: init,
        Login: login,
        Logout:logout,
        SendMessage2Support: sendMessage2Support,
        OpenChatUser2User: openChatUser2User
    }
})();