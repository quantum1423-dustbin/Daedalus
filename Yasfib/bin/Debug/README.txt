/*********************************************************************
 *
 * File        :  $Source: /cvsroot/ijbswa/current/README,v $
 *
 * Purpose     :  README file to give a short intro.
 *
 * Copyright   :  Written by and Copyright (C) 2001-2010 the
 *                Privoxy team. http://www.privoxy.org/
 *
 *                Based on the Internet Junkbuster originally written
 *                by and Copyright (C) 1997 Anonymous Coders and 
 *                Junkbusters Corporation.  http://www.junkbusters.com
 *
 *                This program is free software; you can redistribute it 
 *                and/or modify it under the terms of the GNU General
 *                Public License as published by the Free Software
 *                Foundation; either version 2 of the License, or (at
 *                your option) any later version.
 *
 *                This program is distributed in the hope that it will
 *                be useful, but WITHOUT ANY WARRANTY; without even the
 *                implied warranty of MERCHANTABILITY or FITNESS FOR A
 *                PARTICULAR PURPOSE.  See the GNU General Public
 *                License for more details.
 *
 *                The GNU General Public License should be included with
 *                this file.  If not, you can view it at
 *                http://www.gnu.org/copyleft/gpl.html
 *                or write to the Free Software Foundation, Inc., 
 *                51 Franklin Street, Fifth Floor, Boston, MA 02110-1301, 
 *                USA
 *
 *********************************************************************/

This README is included with Privoxy 3.0.16. See http://www.privoxy.org/ for
more information. The current code maturity level is "stable".

-------------------------------------------------------------------------------

Privoxy is a non-caching web proxy with advanced filtering capabilities for
enhancing privacy, modifying web page data and HTTP headers, controlling
access, and removing ads and other obnoxious Internet junk. Privoxy has a
flexible configuration and can be customized to suit individual needs and
tastes. It has application for both stand-alone systems and multi-user
networks.

Privoxy is Free Software and licensed under the GNU GPLv2.

Privoxy is an associated project of Software in the Public Interest (SPI).

Helping hands and donations are welcome:

  * http://www.privoxy.org/faq/general.html#PARTICIPATE

  * http://www.privoxy.org/faq/general.html#DONATE

-------------------------------------------------------------------------------

1. IMPORTANT CHANGES

February 2010, Privoxy 3.0.16 stable is released.

This is the first stable release since 3.0.12. It mainly contains bugfixes for
the previous betas which introduced IPv6 support, improved keep-alive support
and a bunch of minor improvements. See the "ChangeLog", and the "What's New"
section and the "Upgrader's Notes" in the User Manual for details.

-------------------------------------------------------------------------------

2. INSTALL

See the INSTALL file in this directory, for installing from raw source, and the
User Manual, for all other installation types.

-------------------------------------------------------------------------------

3. RUN

privoxy [--help] [--version] [--no-daemon] [--pidfile PIDFILE] [--user USER
[.GROUP]] [--chroot] [--pre-chroot-nslookup HOSTNAME ][config_file]

See the man page or User Manual for an explanation of each option, and other
configuration and usage issues.

If no config_file is specified on the command line, Privoxy will look for a
file named 'config' in the current directory (except Win32 which will look for
'config.txt'). If no config_file is found, Privoxy will fail to start.

-------------------------------------------------------------------------------

4. CONFIGURATION

See: 'config', 'default.action', 'user.action', 'default.filter', and
'user.filter'. 'user.action' and 'user.filter' are for personal and local
configuration preferences. These are all well commented. Most of the magic is
in '*.action' files. 'user.action' should be used for any actions
customizations. On Unix-like systems, these files are typically installed in /
etc/privoxy. On Windows, then wherever the executable itself is installed.
There are many significant changes and advances from earlier versions. The User
Manual has an explanation of all configuration options, and examples: http://
www.privoxy.org/user-manual/.

Be sure to set your browser(s) for HTTP/HTTPS Proxy at <IP>:<Port>, or whatever
you specify in the config file under 'listen-address'. DEFAULT is
127.0.0.1:8118. Note that Privoxy ONLY proxies HTTP (and HTTPS) traffic. Do not
try it with FTP or other protocols for the simple reason it does not work.

The actions list can be configured via the web interface accessed via http://
p.p/, as well other options.

-------------------------------------------------------------------------------

5. DOCUMENTATION

There should be documentation in the 'doc' subdirectory. In particular, see the
User Manual there, the FAQ, and those interested in Privoxy development, should
look at developer-manual.

The source and configuration files are all well commented. The main
configuration files are: 'config', 'default.action', and 'default.filter'.

Included documentation may vary according to platform and packager. All
documentation is posted on http://www.privoxy.org, in case you don't have it,
or can't find it.

-------------------------------------------------------------------------------

6. CONTACTING THE DEVELOPERS, BUG REPORTING AND FEATURE REQUESTS

We value your feedback. In fact, we rely on it to improve Privoxy and its
configuration. However, please note the following hints, so we can provide you
with the best support:

-------------------------------------------------------------------------------

6.1. Get Support

For casual users, our support forum at SourceForge is probably best suited:
http://sourceforge.net/tracker/?group_id=11118&atid=211118

All users are of course welcome to discuss their issues on the users mailing
list, where the developers also hang around.

Please don't sent private support requests to individual Privoxy developers,
either use the mailing lists or the support trackers.

Note that the Privoxy mailing lists are moderated. Posts from unsubscribed
addresses have to be accepted manually by a moderator. This may cause a delay
of several days and if you use a subject that doesn't clearly mention Privoxy
or one of its features, your message may be accidentally discarded as spam.

If you aren't subscribed, you should therefore spend a few seconds to come up
with a proper subject. Additionally you should make it clear that you want to
get CC'd. Otherwise some responses will be directed to the mailing list only,
and you won't see them.

-------------------------------------------------------------------------------

6.2. Reporting Problems

"Problems" for our purposes, come in two forms:

  * Configuration issues, such as ads that slip through, or sites that don't
    function properly due to one Privoxy "action" or another being turned "on".

  * "Bugs" in the programming code that makes up Privoxy, such as that might
    cause a crash.

-------------------------------------------------------------------------------

6.2.1. Reporting Ads or Other Configuration Problems

Please send feedback on ads that slipped through, innocent images that were
blocked, sites that don't work properly, and other configuration related
problem of default.action file, to http://sourceforge.net/tracker/?group_id=
11118&atid=460288, the Actions File Tracker.

New, improved default.action files may occasionally be made available based on
your feedback. These will be announced on the ijbswa-announce list and
available from our the files section of our project page.

-------------------------------------------------------------------------------

6.2.2. Reporting Bugs

Please report all bugs through our bug tracker: http://sourceforge.net/tracker
/?group_id=11118&atid=111118.

Before doing so, please make sure that the bug has not already been submitted
and observe the additional hints at the top of the submit form. If already
submitted, please feel free to add any info to the original report that might
help to solve the issue.

Please try to verify that it is a Privoxy bug, and not a browser or site bug or
documented behaviour that just happens to be different than what you expected.
If unsure, try toggling off Privoxy, and see if the problem persists.

If you are using your own custom configuration, please try the stock configs to
see if the problem is configuration related. If you're having problems with a
feature that is disabled by default, please ask around on the mailing list if
others can reproduce the problem.

If you aren't using the latest Privoxy version, the bug may have been found and
fixed in the meantime. We would appreciate if you could take the time to
upgrade to the latest version (or even the latest CVS snapshot) and verify that
your bug still exists.

Please be sure to provide the following information:

  * The exact Privoxy version you are using (if you got the source from CVS,
    please also provide the source code revisions as shown in http://
    config.privoxy.org/show-version).

  * The operating system and versions you run Privoxy on, (e.g. Windows XP
    SP2), if you are using a Unix flavor, sending the output of "uname -a"
    should do, in case of GNU/Linux, please also name the distribution.

  * The name, platform, and version of the browser you were using (e.g.
    Internet Explorer v5.5 for Mac).

  * The URL where the problem occurred, or some way for us to duplicate the
    problem (e.g. http://somesite.example.com/?somethingelse=123).

  * Whether your version of Privoxy is one supplied by the Privoxy developers
    via SourceForge, or if you got your copy somewhere else.

  * Whether you are using Privoxy in tandem with another proxy such as Tor. If
    so, please temporary disable the other proxy to see if the symptoms change.

  * Whether you are using a personal firewall product. If so, does Privoxy work
    without it?

  * Any other pertinent information to help identify the problem such as config
    or log file excerpts (yes, you should have log file entries for each action
    taken). To get a meaningful logfile, please make sure that the logfile
    directive is being used and the following debug options are enabled:

    debug     1 # Log the destination for each request Privoxy let through. See also debug 1024.
    debug     2 # show each connection status
    debug     4 # show I/O status
    debug     8 # show header parsing
    debug   128 # debug redirects
    debug   256 # debug GIF de-animation
    debug   512 # Common Log Format
    debug  1024 # Log the destination for requests Privoxy didn't let through, and the reason why.
    debug  4096 # Startup banner and warnings.
    debug  8192 # Non-fatal errors

    If you are having trouble with a filter, please additionally enable

    debug    64 # debug regular expression filters

    Note that Privoxy log files may contain sensitive information so please
    don't submit any logfiles you didn't read first. You can mask sensitive
    information as long as it's clear that you removed something.

You don't have to tell us your actual name when filing a problem report, but if
you don't, please use a nickname so we can differentiate between your messages
and the ones entered by other "anonymous" users that may respond to your
request if they have the same problem or already found a solution.

Please also check the status of your request a few days after submitting it, as
we may request additional information. If you use a SF id, you should
automatically get a mail when someone responds to your request.

The appendix of the Privoxy User Manual also has helpful information on
understanding actions, and action debugging.

-------------------------------------------------------------------------------

6.3. Request New Features

You are welcome to submit ideas on new features or other proposals for
improvement through our feature request tracker at http://sourceforge.net/
tracker/?atid=361118&group_id=11118.

-------------------------------------------------------------------------------

6.4. Other

For any other issues, feel free to use the mailing lists. Technically
interested users and people who wish to contribute to the project are also
welcome on the developers list! You can find an overview of all Privoxy-related
mailing lists, including list archives, at: http://sourceforge.net/mail/?
group_id=11118.

