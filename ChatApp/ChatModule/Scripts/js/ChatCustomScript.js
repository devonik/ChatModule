/**
 * Created by Niklas Grieger on 02.12.2016.
 * js for the Chat Widget
 */
/// <var>The chat javascript as a global variabel</var>
var Chat = Chat || (function () {
    var allCategoriesDataSource;
    var allUserDatasource;
    var empfaenger_name;
    var currentUserType = "";
    var currentUserDataSource = null;
    var adminId = 0;
    var empfaenger_id;
    var partial;
    // Reference the auto-generated proxy for the hub.  
    var chat = $.connection.chatHub;
    var notification = $.connection.notificationHub;
    var user = $.connection.userHub;
    //Füllt die Kontaktlist von dem Normal User
    function fillUserlist() {
        /// <summary>
        /// Fills the friendlist.
        /// </summary>
        var contactlist = [];
        if (partial == "True") {
            var url = "/Chat/Chatlogs/GetAllSubjects";
        }
        else {
            var url = "/Chatlogs/GetAllSubjects";
        }
        $("#top h5").html("Zu welcher Kategorie haben Sie eine Frage ? Wählen Sie:");
        //Get the Contact List
        $.ajax({
            url: url,
            success: function (data) {
                $.each(data,
                    function (i, item) {
                        //Jeder User der aus der Kontaktliste wird dem Array hinzugefügt
                        var contact = "<div id='" + item.supportgroup_id + "' class='friend'>" +
                                                        "<p>" +
                                                            "<strong>" + item.subject + "</strong></br>" +
                                                        "</p>" +
                                                    "</div>";
                        contactlist.push(contact);
                    });
                supportGroups = data;
                //Das Komplette Array wird dem HTML element hinzugefügt
                $("#friends").html(contactlist);
            },
            cache: false
        });
    }
    //Füllt die Kontaktlist von dem Supporter
    function fillSupportList() {
        /// <summary>
        /// Fills the friendlist.
        /// </summary>
        var contactlist = [];
        //Pfad anpassung, wenn es als Partial View geladen wird oder nicht
        if (partial == "True") {
            var url = "/Chat/Users/GetContactListForAdmin?currentUserId=" + currentUserDataSource.user_id;
        }
        else {
            var url = "/Users/GetContactListForAdmin?currentUserId=" + currentUserDataSource.user_id;
        }
        $("#top h5").html("Ihre bestehenden Kontaktanfragen:");
        //Get the Contact List
        $.ajax({
            url: url,
            success: function (data) {
                var status = "";
                $.each(data,
                    function (i, item) {
                        //Verhindert, dass der CurrentUser in der Liste erscheint
                        //Jeder User der aus der Kontaktliste wird dem Array hinzugefügt
                        if (item.user_id != currentUserDataSource.user_id) {
                            var contact = "<div id='" + item.user_id + "' class='friend userheader'>" +
                                                    "<img src='" + item.avatarlink + "' alt='Kein Bild!'/>" +
                                                        "<span id='user_name'>" + item.first_name + " " + item.last_name + "</span>" +
                                                        "<div class='status  "+ item.status + "'></div>" +
                                                    "</div>";
                            contactlist.push(contact);
                        }
                    });
                //Das Komplette Array wird dem HTML element hinzugefügt
                $("#friends").html(contactlist);
            },
            cache: false
        });
    }
    //Lädt die Messages nach dem Auswählen eines Kontakt
    function loadMessagesUser2User() {
        
        var timenow = kendo.toString(new Date, "dd.MM.yyyy HH:mm:ss");
        var chatlogu2u = [];
        var sender_id = currentUserDataSource.user_id;
        var dataString = 'sender_id=' + sender_id + '&empfaenger_id=' + empfaenger_id;
        //Pfad anpassung, wenn es als Partial View geladen wird oder nicht
        if (partial == "True") {
            var url = "/Chat/Chatlogs/GetChatUser2User";
        }
        else {
            var url = "/Chatlogs/GetChatUser2User";
        }
        //Get the Messages by User 2 User
            $.ajax({
                type: 'GET',
                data: dataString,
                url: url,
                success: function (data) {
                    var chatlogu2uItem = "";
                    var today = kendo.toString(new Date(), "dd. MMMM");
                    var todayDay = kendo.toString(new Date(), "d");
                    //Fügt dem Array den Heutigen Tag hinzu
                    chatlogu2uItem = "<label>" + today + "</label>";
                    chatlogu2u.push(chatlogu2uItem);
                    $.each(data,
                        function (i, item) {
                            if (item.message !== "") {
                                var chatlogu2uItem = "";
                                var timeFromNow = kendo.toString(kendo.parseDate(item.timestamp, "H:mm"), "H:mm");
                                // Wenn der Current User der Empfänger der Nachricht ist, wird die Nachricht links angeordnet
                                if (currentUserDataSource.user_id == item.empfaenger_id) {
                                    chatlogu2uItem = "<div class='message'>" +
                                                        "<img src='' />" +
                                                        "<div class='bubble'> " +
                                                            item.message +
                                                            "<div class='corner'></div> " +
                                                            "<span>" + timeFromNow + "</span>" +
                                                        "</div>" +
                                                     "</div>";
                                }
                                    //Wenn der Current User der Sender der Nachricht ist, wird die Nachricht rechts angeordnet
                                else if (item.sender_id == currentUserDataSource.user_id) {
                                    chatlogu2uItem = "<div class='message right'>" +
                                                        "<div class='bubble'>" +
                                                            item.message +
                                                            "<div class='corner'></div>" +
                                                            "<span>" + timeFromNow + "</span>" +
                                                        "</div>" +
                                                     "</div>";
                                }
                                //Fügt die Nachricht dem Array hinzu
                                chatlogu2u.push(chatlogu2uItem);
                            }

                        });
                    //Fügt das komplette Array dem HTML element hinzu
                    $("#chat-messages").html(chatlogu2u);
                },
                //Wenn nicht angegeben kommt es im IE zu Problemen: Wenn eine Message geschickt wird und dann der Chat geöffnet wird wurde die Message nicht angezeigt
                cache: false
            });
        //Scrollt nach unten beim ersten mal laden des Chats
        setTimeout(function () {
            $("#chat-messages").scrollTop($("#chat-messages").prop("scrollHeight"));
        }, 100);
    }
    //Speichert ein Message in der Datenbank und sendet diese an den SignalR Hub
    //Wird ausgeführt, sobald auf den Senden Button geklickt wurde oder die Enter Taste in der Message Box ausgeführt wurde
    function sendMessage() {
        var validator = $("#messageForm").data("kendoValidator");
        console.log("IM sendMessage()");
        /// <summary>
        /// Sends the message user2 user.
        /// </summary>
        var timeNow = kendo.toString(new Date(), "H:mm");
        var sender_id = currentUserDataSource.user_id;
        var sender_name = currentUserDataSource.full_name;
        //Message ohne jegliches html
        var decodedMsg = "";
        console.log("tru kendo editor value:");
        console.log($("#messageeditor").data("kendoEditor").value());
        var decodedMsg = $('<div />').html($("#messageeditor").data("kendoEditor").value()).text();
        //Encodiert den Text as dem Editor in &...
        var encodedMsg = $('<div />').text($("#messageeditor").data("kendoEditor").value()).html();
        //Encodiert erneut um es an die Methode senden zu können
        var encodedURIMessage = encodeURIComponent(encodedMsg);
        console.log("encodedURIMessage");
        console.log(encodedURIMessage);

        
        if (validator.validate()) {
            console.log("decodedMsg im if block");
            console.log(decodedMsg);
            //Start Hub to Send something
            $.connection.hub.start().done(function () {
                // Call the Send method on the hub. 
                console.log("SignalR an Server: send message getriggert");
                chat.server.send(sender_id, empfaenger_id, $("#messageeditor").data("kendoEditor").value(), timeNow);
                console.log("SignalR an Server: sendNotification getriggert");
                notification.server.sendNotification("Neue Message von: " + sender_name, sender_id, empfaenger_id);

            }).fail(function (reason) {
                console.log("SignalR connection in sendMessage und/oder sendNotification failed: " + reason);
            });;

            //Pfad anpassung, wenn es als Partial View geladen wird oder nicht
            if (partial == "True") {
                var url = "/Chat/Chatlogs/SendMessage";
            }
            else {
                var url = "/Chatlogs/SendMessage";
            }
            var dataString = 'sender_id=' + sender_id + '&empfaenger_id=' + empfaenger_id + '&message=' + encodedURIMessage;
            $.ajax({
                type: 'POST',
                //contentType: "application/x-www-form-urlencoded; charset=UTF-8",
                data: dataString,
                url: url,
                success: function (data) {
                    $("#messageeditor").data("kendoEditor").value("");
                    $('#messageeditor').val('').focus();
                },
                error: function (e) {
                    console.log(e);
                }
            });

        }

        
    }
    //Fügt der Oberfläche funktionen zur Darstellung hinzu. Je nach Usertyp.
    function toggleChat() {
        /// <summary>
        /// Setzt den Focus auf das Suchfeld oder die MessageBox 
        /// Animiert beim Klick auf ein Kontakt den Übergang zum Privaten Chat
        /// </summary>

        //Click event beim auswählen eines Kontakts
        setTimeout(function () {
            $(".friend")
                .each(function () {
                    $(this)
                        .click(function () {

                            //Anzeige, wenn der User kein Admin/Supporter ist
                            if (currentUserType == "normal") {
                                subject = $(this).find("p strong").html();
                                $("#window_chooseSupport").kendoWindow({
                                    modal: true,
                                    title: "Wählen Sie Ihren Ansprechpartner",
                                    width: 420
                                });
                                $('#grid_chooseSupport').kendoGrid({
                                    dataSource: {
                                        transport: {
                                            read: {
                                                url: "/Chat/Users/GetSupportsBySubject?subject=" + subject,
                                                success: function (e) {
                                                    console.log(e);
                                                    console.log("succes get data to grid");
                                                    console.log(data);
                                                },
                                                error:function(e){
                                                    console.log("error weil");
                                                    console.log(e);
                                                }
                                            }
                                        }
                                    },
                                    width: 400,
                                    selectable: "row",
                                    scrollable: false,
                                    columns: [{
                                        template: "<div style='display:inline-block;'><img class='gridavatar' src='#:data.avatarlink#' alt='Kein Bild gefunden!'/></div>" +
                                                    "<div class='griduser-name'>#: user_name #</div>",
                                        field: "user_name",
                                        title: "Name",
                                        width: 200
                                    }, {
                                        template: "<div class='status #:data.status#' style='margin-bottom:15px;'></span>",
                                        field: "status",
                                        title: "Status",
                                        width: 100
                                    }],
                                    change: function(e) {
                                        var selectedRows = this.select();
                                        console.log("selected: ");
                                        console.log(e);
                                        console.log(selectedRows);
                                        var selectedDataItems = [];
                                        //GET THE SELECTED DATA ITEM(S) WHEN USING ROW SELECTION
                                        for (var i = 0; i < selectedRows.length; i++) {
                                            var dataItem = this.dataItem(selectedRows[i]);
                                            selectedDataItems.push(dataItem);
                                        }
                                        // selectedDataItems contains all selected data items
                                        empfaenger_name = selectedDataItems[0].user_name;
                                        console.log("empfänger_name: " + empfaenger_name);
                                        empfaenger_id = selectedDataItems[0].supporter_id;
                                        empfaenger_status = selectedDataItems[0].status;
                                        empfaenger_avatar = selectedDataItems[0].avatarlink;

                                        setTimeout(function () {
                                            $("#chat-messages").addClass("animate");
                                        },150);

                                        //Setzt den Profil Header im Chat
                                        var profilHtml = "<div class='userheader " + empfaenger_id + "'>" +
                                                                "<a href='#' id='close' data-toggle='tooltip' data-placement='bottom' title='Zurück zur Kontaktliste'></a>" +
                                                                "<img class='avatar' src='" + empfaenger_avatar + "' alt='Kein Bild gefunden!' style='margin:10px;left:14px;position:relative;'/></br>" +
                                                                "<div class='status " + empfaenger_status + "' style='left:50%; position:relative;'></div></br>" +
                                                                "<b>Sie sprechen mit: " + empfaenger_name + "</b>"+
                                                            "</div>";
                                        $("#profile").html(profilHtml);
                                        //Initialisiert den toggle zur Anzeige von Tooltips
                                        $('#close').tooltip();

                                        //Schließt das Window und zeigt den ChatView an
                                        $("#window_chooseSupport").data("kendoWindow").close();
                                        setTimeout(function () {
                                            //Scrollt zur letzten Nachricht
                                            $("#chat-messages").scrollTop($("#chat-messages").prop("scrollHeight"));
                                        }, 10);
                                        $('#friendslist').fadeOut();
                                        $('#chatview').fadeIn();
                                        $('#chatview').css("display", "flex");
                                        $('#chatview').css("flex-direction", "column");

                                        

                                        //Funktion zum schließen des Chats und Anzeige der Kontaktliste
                                        $('#close')
                                        .unbind("click")
                                        .click(function () {
                                                $("#chat-messages, #profile, #profile p").removeClass("animate");
                                                setTimeout(function () {
                                                    $('#chatview').fadeOut();
                                                    $('#friendslist').fadeIn();
                                                },
                                                    50);
                                            });
                                            //Lädt die bisherigen Messages aus der Datenbank
                                        loadMessagesUser2User();
                                        


                                    }
                                    
                                });
                                var grid = $("#grid_chooseSupport").data("kendoGrid");
                                var window = $("#window_chooseSupport").data("kendoWindow");
                                //apply the activate event, which is thrown only after the animation is played out
                                window.one("activate", function () {
                                    grid.resize();
                                });
                                ////Beim Klick auf das Thema in der Kontaktliste, wird das Window geöffnet und das grid angezeigt
                                window.center().open();
                                $("#grid_chooseSupport").css("display", "");
                                
                            }
                            //Anzeige, wenn der CurrentUser ein Supporter ist
                            else if(currenUserType="admin"){
                                empfaenger_name = $(this).find("span#user_name").text();
                                empfaenger_id = $(this).attr('id');
                                empfaenger_avatar = $(this).find("img").attr("src");
                                empfaenger_status = $(this).find("div.status").attr("class");
                                sender_id = currentUserDataSource.user_id;

                                setTimeout(function () {
                                    $("#profile p").addClass("animate");
                                    $("#profile").addClass("animate");
                                }, 100);

                                setTimeout(function () {
                                    $("#chat-messages").addClass("animate");
                                },  150);

                                //Setzt den Profil Header im Chat
                                var profilHtml = "<div class='userheader "+empfaenger_id+"'>"+
                                                    "<a href='#' id='close' data-toggle='tooltip' data-placement='bottom' title='Zurück zur Kontaktliste'></a>" +
                                                    "<img class='avatar' src='" + empfaenger_avatar + "' alt='Kein Bild gefunden!' style='margin:10px;left:14px;position:relative;'/></br>" +
                                                    "<div class='status " + empfaenger_status + "' style='left:50%; position:relative;'></div></br>" +
                                                    "<b>Sie sprechen mit: " + empfaenger_name + "</b>"
                                "</div>";
                                
                                
                                $("#profile").html(profilHtml);
                                $("#profile").append("<button class='btn-primary' style='margin:10px;' onclick='javascript:Chat.CloseRequest(" + sender_id + "," + empfaenger_id + ");'>Anfrage schließen</button>");
                                //Schließt die Kontaktliste und zeigt den ChatView an
                                
                                //Initialisiert den toggle zur Anzeige von Tooltips
                                $('#close').tooltip();
                                setTimeout(function () {
                                    //Scrollt zur letzten Nachricht
                                    $("#chat-messages").scrollTop($("#chat-messages").prop("scrollHeight"));
                                }, 10);
                                $('#friendslist').fadeOut();
                                $('#chatview').fadeIn();
                                $('#chatview').css("display", "flex");
                                $('#chatview').css("flex-direction", "column");
                                
                                
                                

                                //Funktion zum schließen des Chats und Anzeige der Kontaktliste
                                $('#close')
                                .unbind("click")
                                .click(function () {
                                    $("#chat-messages, #profile, #profile p").removeClass("animate");
                                    setTimeout(function () {
                                        $('#chatview').fadeOut();
                                        $('#friendslist').fadeIn();
                                        $("#top").show();
                                    },
                                        200);
                                });
                                //Lädt die bisherigen Messages aus der Datenbank
                                console.log("loadMessagesUser2User with CurrentUserType=admin");
                                loadMessagesUser2User();
                            }
                            
                        });

                });
        }, 600);
    }
    function closeRequest(sender_id,empfaenger_id) {
        $.ajax({
            url: "/Chat/Chatlogs/ClearChat?sender_id=" + sender_id + "&empfaenger_id=" + empfaenger_id,
            success: function (e) {
                console.log("Chat zwischen sender:" + sender_id + " und empfaenger:" + empfaenger_id+" gelöscht");
            }
        });
        loadMessagesUser2User();
    }
    //Checkt, ob der CurrentUser ein Admin oder Normaler User ist, je nach dem wird die Kontaktliste gefüllt
    function checkUserIsAdmin(currentUserId) {
        if (partial == "True") {
            var url = "/Chat/Users/CheckUserIsAdmin?user_id=" + currentUserId;
        }
        else {
            var url = "/Users/CheckUserIsAdmin?user_id=" + currentUserId;
        }
        $.ajax({
            url: url,
            success: function (data) {
                if (data === "False") {
                    fillUserlist();
                    currentUserType="normal";
                }
                else if (data === "True") {
                    fillSupportList();
                    currentUserType="admin";
                }
            }
        })
    }
    //Öffnet den Chat, nachdem in der Notification auf die Nachricht geklickt wurde
    function openChatUser2User(senderId, empfaengerId) {
            console.log("Öffne Chat...");
            $(".friend#" + senderId).find("div").trigger("click");
            setTimeout(function () {
                $("#chatWindow").data("kendoWindow").center().open();
                
            }, 500);
    }
    //Liest die Messages aus der Datenbank, die der User bekommen hat während er offline wahr. Diese werden als Notfication der Oberfläche hinzugefügt
    function getMessagesSinceLastLogout(currentUserIdParam) {
        //Pfad anpassung, wenn es als Partial View geladen wird oder nicht
        if (partial == "True") {
            var url = "/Chat/Chatlogs/GetMessagesSinceLastLogin?currentUserId=" + currentUserIdParam;
        }
        else {
            var url = "/Chatlogs/GetMessagesSinceLastLogin?currentUserId=" + currentUserIdParam;
        }
        $.ajax({
            type: "GET",
            url: url,
            success: function (data) {
                var count = 0;
                if (data.length > 0) {
                    
                    $.each(data, function (index, value) {
                        var encodedMsg = $('<div />').text(value.message).html();
                        $('#notiContent').append($("<li>"+
                                                        "<a style='text-decoration:none !important;' href='javascript:Chat.OpenChatUser2User(" + value.sender_id + ", " + value.empfaenger_id + ");'>" +
                                                            "Neue Message von: " + value.sender_name+"</br>"+
                                                            "Message: " + kendo.toString(kendo.parseDate(value.timestamp, "H:mm"), "H:mm") + "-> " + encodedMsg +
                                                        "</a>"+
                                                   "</li>"));
                        count++;
                    });
                }
                else {
                    $('#notiContent').html($('<li>Keine Neuigkeiten</li>'));
                }
                $('span.count').html(count);
                
            }
        })
    }
    //Zählt die Anzahl der Notifications
    function updateNotificationCount() {
        var count = 0;
        count = parseInt($('span.count').html()) || 0;
        count++;
        $('span.count').html(count);
    }
    /*Liest Daten aus der Datenbank zu dem aktuellen User
    Beim erfolg, werden die Messages seit dem letzten Logout abgefragt 
    Beim erfolg, wird geprüft, ob der User ein Admin ist
    Beim erfolg, wird die Oberfläche entsprechend des Usertyps angezeigt*/
    function getInfoByCurrentUser(currentUserId) {
        var urlCurrentUser = "/Chat/Chatlogs/GetUserInfoById?currentUserId=" + currentUserId;
        //Liest Daten aus der Datenbank zu dem aktuellen User
        $.ajax({
            type: "GET",
            url: urlCurrentUser,
            success: function (data) {
                currentUserDataSource = data;
                //Die Messages seit dem letzten Logout werden abgefragt, und als Notifications angezeigt
                getMessagesSinceLastLogout(currentUserId);
                //Es wird geprüft, ob der User ein Admin ist
                checkUserIsAdmin(currentUserId);
                $("#top h4").html("Willkommen im Chat: " + currentUserDataSource.full_name);
                $("#top").append("<hr/>");
                //Die Oberfläche entsprechend des Usertyps geladen
                toggleChat();
            },
            error: function (e) {
                console.log("Userinfos konnten für den User mit der ID: " + currentUserIdParam + " nicht geladen werden.");
            }
        });
    }
    //Initialisiert die SignalR Methoden zum Triggern an den Client
    function signalRToClient() {
        chat.client.addNewMessageToPage = function (sender_id, empfaenger_id, message, timestamp) {
            // Html encode display name and message. 
            $('#isTyping').html('');
            console.log("SignalR and Client: addNewMessageToPage getriggert...");
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
            $("#chat-messages").scrollTop($("#chat-messages").prop("scrollHeight"));
        }
        //Zum Real time Anzeigen [User] is Typing....
        chat.client.sayWhoIsTyping = function (name, sender_id, empfaenger_id) {
            //Fügt es nur dem Empfänger hinzu
            if (currentUserDataSource.user_id == empfaenger_id) {
                $('#isTyping').html(name + " schreibt...");
            }
        }
        //Fügt dem User eine Notification in echtzeit hinzu
        notification.client.getNotification = function (notification, sender_id, empfaenger_id) {
            console.log("SignalR an Client: getNotification getriggert...");
            //Fügt die Notification dem gegenüber nur dem empfänger hinzu UND nur wenn der Chat geschlossen ist
            if (empfaenger_id == currentUserDataSource.user_id && !$("#chatContent").is(':visible')) {
                updateNotificationCount();
                $('#notiContent').html('');
                $('#notiContent').append("<a href='javascript:Chat.OpenChatUser2User(" + sender_id + ", " + empfaenger_id + ");'>" + $.notify(notification, "info") + "</a>");
            }
        };
        ////Updatet den User Status in der Oberfläche, sobald der SignalR Hub getriggert wurde
        user.client.getRealTimeStatus = function (user_id, status) {
            console.log("SignalR an Client: getRealTimeStatus getriggert...");
            console.log("User mit ID: " + user_id + " ist nun " + status);
            $(".userheader." + user_id).find(".status").attr('class', 'status ' + status);
            $(".userheader." + user_id).find(".status").notify("User ist nun " + status,"info");
            console.log("1");
        };
    }
    //Setzt den CurrentUser auf den Status available/online
    function setUserOnline(currentUserId) {
        var urlSetUserOnline = "/Chat/Chatlogs/SetUserStatus?status=" + 'available' + "&currentUserId=" + currentUserId;
        //Updatet die Datenbank und setzt den User Status auf available/online
        $.ajax({
            type: "POST",
            url: urlSetUserOnline,
            success: function () {
                console.log("user ist jetzt online/available");
            }
        });
        //Startet eine connection zum SignalR Hub
        $.connection.hub.start().done(function () {
            console.log("SignalR an Server: realTimeStatus getriggert...");
            //Sendet den Status des Users an dem SignalR Hub
            user.server.realTimeStatus(currentUserId, 'available');
            
        }).fail(function (reason) {
            console.log("SignalR connection in realTimeStatus(online) failed: " + reason);
        });
    }
    //Setzt den CurrentUser auf den Status inactiv/offline, sobald die Page geschlossen wird
    function setUserOffline(currentUserId){
        //Triggert, wenn die Seite geschlossen wird
        $(window).bind('beforeunload', function () {
            //Startet eine connection zum SignalR Hub
            $.connection.hub.start().done(function () {
                console.log("SignalR an Server: realTimeStatus getriggert...");
                //Sendet den Status des Users an dem SignalR Hub
                user.server.realTimeStatus(currentUserId, 'inactive');
                
            }).fail(function (reason) {
                console.log("SignalR connection in realTimeStatus(offline) failed: " + reason);
            });
            //Updatet die Datenbank und setzt den User Status auf inactiv/offline
            $.ajax({  
                type: 'POST',
                url: "/Chat/Chatlogs/SetUserStatus?status=inactive&currentUserId=" + currentUserId,
                success: function () {
                    console.log("Der Timestamp vom Userlogin ID=" + currentUserId + " wurde geändert");
                    console.log("user ist jetzt offline/inactive");
                },
                error: function (e) {
                    console.log("Userlogin ID=" + currentUserId + " Timestamp wurde nicht gesetzt");
                    console.log(e);
                }
            });
                
        });
    }
    function kendoValidator() {
        var container = $("#messageForm");
        container.kendoValidator({
            validateOnBlur: false,
            rules: {
                maxTextLength: function (textarea) {
                    if (textarea.data("kendoEditor").value() != "") {
                        
                        var maxlength = "200";
                        var value = textarea.data("kendoEditor").value();
                        console.log("message länge ohne tags");
                        console.log(value.replace(/<[^>]+>/g, "").length <= maxlength);
                        return value.replace(/<[^>]+>/g, "").length <= maxlength;
                    }
                    return true;
                },
                isValueEmpty: function (textarea) {
                    value = textarea.data("kendoEditor").value().replace(/<[^>]+>/g, "");
                    if (value == "") {
                        return false;
                    }
                    return true;
                },
                hasScriptTag: function (textarea) {
                    var value = $('<div />').text($("#messageeditor").data("kendoEditor").value()).html();
                    if (value.indexOf('script') >= 0) {
                        return false;
                    }
                    return true;
                }
            },
            messages: {
                maxTextLength: "Es sind nur bis zu 200 Zeichen erlaubt",
                isValueEmpty: "Die Nachricht ist leer",
                hasScriptTag: "Kein Scripting erlaubt"
            }
        });
        var validator = $("#messageForm").data("kendoValidator");
        $("body").click(function () {
            validator.hideMessages();
        });
    }
    //Init 
    function init(currentUserIdParam, Partial) {
        var defaultTools = kendo.ui.Editor.defaultTools;

        //Zum setzen eines Absatzes in der Message = shift+Enter
        defaultTools["insertLineBreak"].options.shift = true;
        //Deaktiviert die funktion zum einfügen eines Paragraphen 
        delete defaultTools["insertParagraph"].options;

        $("#messageeditor").kendoEditor({
            tools: [
              //"bold", "italic", "underline"
            ],
            //Wird getriggert, wenn ein Item in der Toolbar geklickt wird
            execute: function(e) {
                console.log("executing command", e.name, e.command);
            },
            keydown: function (e) {
                $.connection.hub.start().done(function () {
                    var encodedName = currentUserDataSource.full_name;
                    //Sendet das Signal, das jemand Schreibt an den Hub (Server)

                    console.log("SignalR an Server: isTyping getriggert");
                    chat.server.isTyping(encodedName, currentUserDataSource.user_id, empfaenger_id, currentUserDataSource.user_id);

                }).fail(function (reason) {
                    console.log("SignalR connection in UserisTyping failed: " + reason);
                });
                if (e.keyCode === 13 && !e.shiftKey) {
                    e.preventDefault();
                        console.log("Enter im Editor gedrückt");
                        sendMessage();
                    }
            }
        });
        kendoValidator();
        //Zum einstellen der Höhe des Kendo Editors
        setTimeout(function () {
            $(".k-editor iframe.k-content").css("height", "100px");
            $("table.k-editor").css("height", "100px");
        },100);
        $('#send').on('click', function (e) {
            e.preventDefault();
            sendMessage();
        });
        partial = Partial;
        if(currentUserIdParam != null){
            //Liest Daten aus der Datenbank zu dem aktuellen User
            //Beim erfolg, werden die Messages seit dem letzten Logout abgefragt 
            //Beim erfolg, wird geprüft, ob der User ein Admin ist
            //Beim erfolg, wird die Oberfläche entsprechend des Usertyps angezeigt
            getInfoByCurrentUser(currentUserIdParam);

            //Enthält die SignalR Methoden zum anzeigen am Client
            signalRToClient();

            //Setzt den CurrentUser auf den Status available/online
            setUserOnline(currentUserIdParam);

            //Setzt den CurrentUser auf den Status inactiv/offline
            setUserOffline(currentUserIdParam);
        
           
                    
    }
    else{

    }
        
    }
    //Return functions
    return {
        Init: init,
        //SendMessage: sendMessage,
        OpenChatUser2User: openChatUser2User,
        CloseRequest:closeRequest
    }
})();